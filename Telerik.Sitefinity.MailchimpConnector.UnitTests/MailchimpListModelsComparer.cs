using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.Test.Unit.Mailchimp
{
    internal static class MailchimpListModelsComparer
    {
        public static bool AreEqual(IEnumerable<MailchimpList> first, IEnumerable<MailchimpList> second)
        {
            if (first == second)
            {
                return true;
            }

            MailchimpList[] firstArray = first.OrderBy(f => f.Id).ToArray();
            MailchimpList[] secondArray = second.OrderBy(f => f.Id).ToArray();

            bool areEqlual = true;

            for (int i = 0; i < first.Count(); i++)
            {
                areEqlual = areEqlual && MailchimpListModelsComparer.AreEqual(firstArray[i], secondArray[i]);
            }

            return areEqlual;
        }

        public static bool AreEqual(MailchimpList first, MailchimpList second)
        {
            bool areEqlual = first.Id == second.Id;
            areEqlual = areEqlual && first.Name == second.Name;

            return areEqlual;
        }
    }
}