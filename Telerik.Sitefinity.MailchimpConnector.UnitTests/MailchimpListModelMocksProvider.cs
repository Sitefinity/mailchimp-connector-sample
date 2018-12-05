using System;
using System.Collections.Generic;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.Test.Unit.Mailchimp
{
    internal static class MailchimpListModelMocksProvider
    {
        public static IEnumerable<MailchimpList> CreateMockedListsCollection(int count)
        {
            IList<MailchimpList> lists = new List<MailchimpList>();

            for (int i = 0; i < count; i++)
            {
                MailchimpList currentList = MailchimpListModelMocksProvider.CreateMockedList();
                lists.Add(currentList);
            }

            return lists;
        }

        public static MailchimpList CreateMockedList()
        {
            MailchimpList mailchimpList = new MailchimpList();
            mailchimpList.Id = Guid.NewGuid().ToString();
            mailchimpList.Name = Guid.NewGuid().ToString();

            return mailchimpList;
        }
    }
}