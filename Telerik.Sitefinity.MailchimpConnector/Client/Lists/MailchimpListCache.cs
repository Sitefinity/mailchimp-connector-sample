using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.MailchimpConnector.Model;
using Telerik.Sitefinity.Services;

namespace Telerik.Sitefinity.MailchimpConnector.Client.Lists
{
    /// <summary>
    /// Contains methods for retrieving cached Mailchimp lists.
    /// </summary>
    internal class MailchimpListCache : IMailchimpListCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpListCache"/> class.
        /// </summary>
        public MailchimpListCache()
            : this(ObjectFactory.Resolve<IMailchimpListClient>(), SystemManager.GetCacheManager(CacheManagerInstance.Global))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpListCache"/> class.
        /// </summary>
        /// <param name="mailchimpListProvider">The <see cref="IMailchimpListProvider"/> implementation that will be used to get non-cached data.</param>
        internal MailchimpListCache(IMailchimpListProvider mailchimpListProvider, ICacheManager cacheManager)
        {
            this.mailchimpListProvider = mailchimpListProvider;
            this.cachedLists = new SynchronizedCache<IEnumerable<MailchimpList>>(cacheManager);
            this.cachedMergeFields = new SynchronizedCache<IEnumerable<MailchimpListMergeField>>(cacheManager);
        }

        /// <inheritdoc/>
        public IEnumerable<MailchimpList> GetLists()
        {
            try
            {
                IEnumerable<MailchimpList> lists =
                    this.cachedLists.GetAndUpdateItem(MailchimpListCache.ListsCacheKey, () => this.mailchimpListProvider.GetLists());

                return lists;
            }
            catch (Exception ex)
            {
                Log.Write(ex, TraceEventType.Error);

                return null;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<MailchimpListMergeField> GetMergeFields(string id)
        {
            try
            {
                IEnumerable<MailchimpListMergeField> mergeFields =
                    this.cachedMergeFields.GetAndUpdateItem(string.Concat(MailchimpListCache.ListsCacheKey, id), () => this.mailchimpListProvider.GetMergeFields(id));

                return mergeFields;
            }
            catch (Exception ex)
            {
                Log.Write(ex, TraceEventType.Error);

                return null;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the managed resources
        /// </summary>
        /// <param name="disposing">Defines whether a disposing is executing now.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.mailchimpListProvider != null)
                {
                    this.mailchimpListProvider.Dispose();
                }
            }
        }

        private readonly IMailchimpListProvider mailchimpListProvider;

        private readonly SynchronizedCache<IEnumerable<MailchimpList>> cachedLists;

        private readonly SynchronizedCache<IEnumerable<MailchimpListMergeField>> cachedMergeFields;

        private const string ListsCacheKey = "MailchimpLists";
    }
}