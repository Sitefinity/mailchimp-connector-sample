using System;
using System.Diagnostics;
using System.Threading;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Services;

namespace Telerik.Sitefinity.MailchimpConnector
{
    /// <summary>
    /// Contains methods for getting and updating(if needed) cached items in thread-safe manner. 
    /// </summary>
    /// <typeparam name="T">The type of cached items</typeparam>
    internal class SynchronizedCache<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCache&lt;T&gt;"/> class.
        /// </summary>
        public SynchronizedCache()
            : this(SystemManager.GetCacheManager(CacheManagerInstance.Global))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCache&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="cacheManager">The <see cref="ICacheManager"/> implementation that will be used to cache the data.</param>
        public SynchronizedCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Gets a cached item and updates the cached item if needed.
        /// </summary>
        /// <param name="key">The key of the item to be retrieved.</param>
        /// <param name="func">The task to invoke in order to retrieve the item to be cached.</param>
        /// <returns>The cached item of type {T}</returns>
        public T GetAndUpdateItem(string key, Func<T> func)
        {
            T result = default(T);
       
            var cachedItem = this.Get(key);

            if (cachedItem == null)
            {
                // First get. Need to return result.
                result = this.UpdateItem(key, func);
            }
            else
            {
                result = cachedItem.Item;
                bool needsRefresh = cachedItem.NeedsRefresh(RefreshIntervalInMin);

                if (needsRefresh)
                {
                    // Can execute in background, since already cached value will be returned.
                    SystemManager.BackgroundTasksService.EnqueueTask(() =>
                    {
                        this.UpdateItem(key, func);
                    });
                }
            }

            return result;
        }

        private T UpdateItem(string key, Func<T> func)
        {
            T updatedValue = default(T);

            if (this.updatingCacheLock.TryEnterWriteLock(LockTimeOut))
            {
                try
                {
                    if (!this.isUpdatingCache)
                    {
                        this.isUpdatingCache = true;
                            
                        T item = null;
                        try
                        {
                            item = func();

                            if (item != null)
                            {
                                this.UpdateCache(key, item);
                                updatedValue = item;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Write(ex, TraceEventType.Error);
                        }
                        finally
                        {
                            this.isUpdatingCache = false;
                        }
                    }
                }
                finally
                {
                    this.updatingCacheLock.ExitWriteLock();
                }
            }
            else
            {
                Log.Write("Write lock cannot be obtained while getting an item from cache, method GetAndUpdateItem.", TraceEventType.Information);
            }

            return updatedValue;
        }

        private void UpdateCache(string key, T item)
        {
            if (this.cacheLock.TryEnterWriteLock(LockTimeOut))
            {
                try
                {
                    if (this.cacheManager.Contains(key))
                    {
                        this.cacheManager.Remove(key);
                    }

                    var itemToCache = new CachedItemWrapper<T>() { Item = item, AddedToCacheTimeStamp = DateTime.UtcNow };
                    this.cacheManager.Add(key, itemToCache);
                }
                finally
                {
                    this.cacheLock.ExitWriteLock();
                }
            }
            else
            {
                Log.Write("Write lock cannot be obtained while updating cache, method UpdateCache.", TraceEventType.Information);
            }
        }

        /// <summary>
        /// Gets the cached item for the specified key.
        /// </summary>
        /// <param name="key">The key of the item to be retrieved.</param>
        /// <returns>The cached instance for the specified key</returns>
        private CachedItemWrapper<T> Get(string key)
        {
            if (this.cacheLock.TryEnterReadLock(LockTimeOut))
            {
                try
                {
                    var result = this.cacheManager[key] as CachedItemWrapper<T>;
                    if (result != null)
                    {
                        return new CachedItemWrapper<T>() { Item = result.Item, AddedToCacheTimeStamp = result.AddedToCacheTimeStamp };
                    }

                    return result;
                }
                finally
                {
                    this.cacheLock.ExitReadLock();
                }
            }
            else
            {
                Log.Write("Lock cannot be obtained while getting cached item.", TraceEventType.Information);
                return null;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SynchronizedCache{T}"/> class.
        /// </summary>
        ~SynchronizedCache()
        {
            if (this.cacheLock != null) this.cacheLock.Dispose();
            if (this.updatingCacheLock != null) this.updatingCacheLock.Dispose();
        }

        private const int RefreshIntervalInMin = 1;
        private const int LockTimeOut = 1000;
        private bool isUpdatingCache = false;
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim updatingCacheLock = new ReaderWriterLockSlim();
        
        private readonly ICacheManager cacheManager;
    }
}