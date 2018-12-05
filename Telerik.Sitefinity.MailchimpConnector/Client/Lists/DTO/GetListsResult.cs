using System.Collections.Generic;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.MailchimpConnector.Client.Lists.DTO
{
    internal class GetListsResult
    {
        /// <summary>
        /// Gets or sets the enumeration of lists.
        /// </summary>
        public IEnumerable<MailchimpList> Lists { get; set; }
    }
}
