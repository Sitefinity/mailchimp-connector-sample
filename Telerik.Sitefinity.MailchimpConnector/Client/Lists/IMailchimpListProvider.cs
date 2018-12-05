using System;
using System.Collections.Generic;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.MailchimpConnector.Client.Lists
{
    /// <summary>
    /// Exposes the interface of Mailchimp list provider.
    /// </summary>
    internal interface IMailchimpListProvider : IDisposable
    {
        /// <summary>
        /// Gets all Mailchimp lists.
        /// </summary>
        /// <returns>The lists list.</returns>
        IEnumerable<MailchimpList> GetLists();

        /// <summary>
        /// Gets all merge fields for a specific list.
        /// </summary>
        /// <param name="id">The id of the list.</param>
        /// <returns>The list of merge fields.</returns>
        IEnumerable<MailchimpListMergeField> GetMergeFields(string id);
    }
}