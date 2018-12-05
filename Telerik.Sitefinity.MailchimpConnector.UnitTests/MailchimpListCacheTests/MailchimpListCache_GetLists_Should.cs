using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Caching;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.BackgroundTasks;
using Telerik.Sitefinity.MailchimpConnector;
using Telerik.Sitefinity.MailchimpConnector.Client.Lists;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.Test.Unit.Mailchimp.MailchimpListCacheTests
{
    /// <summary>
    /// Mailchimp form client MailchimpListCache GetLists unit tests.
    /// </summary>
    [TestClass]
    public class MailchimpListCache_GetLists_Should
    {
        /// <summary>
        /// Initializes the logic used before each of the tests in the test class run.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mailchimpListClient = Mock.Create<IMailchimpListClient>();
            this.cacheManager = this.CreateCacheManagerMock();
        }

        /// <summary>
        /// Tests whether the method will return a collection when calling the method for the first time.
        /// </summary>
        [TestMethod]
        public void ReturnCollection_WhenCallingGetForTheFirstTime()
        {
            // Arrange
            IEnumerable<MailchimpList> expectedLists = MailchimpListModelMocksProvider.CreateMockedListsCollection(5);
            Mock.Arrange(() => this.mailchimpListClient.GetLists()).Returns(expectedLists);

            IMailchimpListCache mailchimpFormsCache = new MailchimpListCache(this.mailchimpListClient, this.cacheManager);
            
            // Act
            IEnumerable<MailchimpList> actualLists = mailchimpFormsCache.GetLists();

            // Assert
            Assert.IsTrue(MailchimpListModelsComparer.AreEqual(expectedLists, actualLists));
        }

        /// <summary>
        /// Tests whether the method will return an empty collection when the original service call returns an empty collection.
        /// </summary>
        [TestMethod]
        public void ReturnEmptyCollection_WhenOriginalCollectionIsEmpty()
        {
            // Arrange
            IEnumerable<MailchimpList> expectedForms = Enumerable.Empty<MailchimpList>();
            Mock.Arrange(() => this.mailchimpListClient.GetLists()).Returns(expectedForms);

            IMailchimpListCache mailchimpFormsCache = new MailchimpListCache(this.mailchimpListClient, this.cacheManager);
            
            // Act
            IEnumerable<MailchimpList> actualForms = mailchimpFormsCache.GetLists();

            // Assert
            Assert.IsTrue(MailchimpListModelsComparer.AreEqual(expectedForms, actualForms));
        }

        /// <summary>
        /// Tests whether the method will return a cached collection after the first get.
        /// </summary>
        [TestMethod]
        public void ReturnCachedCollection_WhenCallingGetAfterFirstTime()
        {
            // Arrange
            IEnumerable<MailchimpList> expectedForms = MailchimpListModelMocksProvider.CreateMockedListsCollection(5);
            Mock.Arrange(() => this.mailchimpListClient.GetLists()).Returns(expectedForms);

            IMailchimpListCache mailchimpFormsCache = new MailchimpListCache(this.mailchimpListClient, this.cacheManager);

            // Act
            mailchimpFormsCache.GetLists();
            Mock.Arrange(() => this.mailchimpListClient.GetLists())
                .Returns(MailchimpListModelMocksProvider.CreateMockedListsCollection(3));
            IEnumerable<MailchimpList> actualForms = mailchimpFormsCache.GetLists();

            // Assert
            Assert.IsTrue(MailchimpListModelsComparer.AreEqual(expectedForms, actualForms));
        }

        /// <summary>
        /// Tests whether the method will update cached collection after cache expires.
        /// </summary>
        [TestMethod]
        public void UpdateCachedCollection_WhenCacheExpires()
        {
            // Arrange
            IEnumerable<MailchimpList> expectedInitialForms = MailchimpListModelMocksProvider.CreateMockedListsCollection(5);
            Mock.Arrange(() => this.mailchimpListClient.GetLists()).Returns(expectedInitialForms);

            var backgroundTaskService = Mock.Create<IBackgroundTasksService>();
            Mock.Arrange(() => backgroundTaskService.EnqueueTask(Arg.IsAny<Action>())).DoInstead((Action action) => { action(); });

            IMailchimpListCache mailchimpFormsCache = new MailchimpListCache(this.mailchimpListClient, this.cacheManager);

            ObjectFactory.RunWithContainer(
                new UnityContainer(),
                () =>
                {
                    // Act
                    ObjectFactory.Container.RegisterInstance<IBackgroundTasksService>(backgroundTaskService);
                    IEnumerable<MailchimpList> actualInitialForms = mailchimpFormsCache.GetLists();

                    this.SetCacheItemsAsExpired();

                    IEnumerable<MailchimpList> expectedUpdatedForms = MailchimpListModelMocksProvider.CreateMockedListsCollection(5);
                    Mock.Arrange(() => this.mailchimpListClient.GetLists()).Returns(expectedUpdatedForms);

                    IEnumerable<MailchimpList> actualExpiredForms = mailchimpFormsCache.GetLists();
                    IEnumerable<MailchimpList> actualUpdatedForms = mailchimpFormsCache.GetLists();

                    // Assert
                    Assert.IsTrue(MailchimpListModelsComparer.AreEqual(expectedInitialForms, actualInitialForms));
                    Assert.IsTrue(MailchimpListModelsComparer.AreEqual(expectedInitialForms, actualExpiredForms));
                    Assert.IsTrue(MailchimpListModelsComparer.AreEqual(expectedUpdatedForms, actualUpdatedForms));
                });
        }

        private ICacheManager CreateCacheManagerMock()
        {
            ICacheManager cacheManager = Mock.Create<ICacheManager>();

            this.cachedObjects = new Dictionary<string, object>();
            Mock.Arrange(() => cacheManager.Add(Arg.AnyString, Arg.AnyObject)).DoInstead((string key, object obj) => { cachedObjects[key] = obj; });
            Mock.Arrange(() => cacheManager[Arg.AnyString]).Returns((string key) =>
            { 
                if (cachedObjects.ContainsKey(key)) 
                { 
                    return cachedObjects[key]; 
                }

                return null;
            });

            return cacheManager;
        }

        private void SetCacheItemsAsExpired()
        {
            if (this.cachedObjects != null && !this.cachedObjects.Any())
            {
                return;
            }

            foreach (KeyValuePair<string, object> item in this.cachedObjects)
            {
                (item.Value as CachedItemWrapper<IEnumerable<MailchimpList>>).AddedToCacheTimeStamp = DateTime.MinValue;
            }
        }

        private IDictionary<string, object> cachedObjects;
        private IMailchimpListClient mailchimpListClient;
        private ICacheManager cacheManager;
    }
}