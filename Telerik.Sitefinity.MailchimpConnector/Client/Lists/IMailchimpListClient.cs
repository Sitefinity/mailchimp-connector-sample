using System;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.MailchimpConnector.Client.Lists
{
    /// <summary>
    /// Exposes the interface of Mailchimp list client API.
    /// </summary>
    internal interface IMailchimpListClient : IMailchimpListProvider, IDisposable
    {
        /// <summary>
        /// Creates a new list member.
        /// </summary>
        /// <param name="id">The id of the list.</param>
        /// <param name="mailchimpListMember">The <see cref="MailchimpListMember"/> member.</param>
        /// <returns></returns>
        MailchimpListMember CreateMember(string id, MailchimpListMember mailchimpListMember);
    }
}