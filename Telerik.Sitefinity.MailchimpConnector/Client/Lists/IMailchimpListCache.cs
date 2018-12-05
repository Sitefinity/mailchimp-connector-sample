using System;

namespace Telerik.Sitefinity.MailchimpConnector.Client.Lists
{
    /// <summary>
    /// Exposes the interface of Mailchimp list client API cache.
    /// </summary>
    internal interface IMailchimpListCache : IMailchimpListProvider, IDisposable
    {
    }
}