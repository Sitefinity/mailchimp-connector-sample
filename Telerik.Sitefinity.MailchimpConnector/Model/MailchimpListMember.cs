using System.Collections.Generic;
using Newtonsoft.Json;
using Telerik.Sitefinity.MailchimpConnector.Enums;

namespace Telerik.Sitefinity.MailchimpConnector.Model
{
    internal class MailchimpListMember
    {
        /// <summary>
        /// Gets or sets the name of the list merge field.
        /// </summary>
        [JsonProperty("email_address")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the list of merge fields.
        /// </summary>
        [JsonProperty("merge_fields")]
        public IDictionary<string, string> MergeFields { get; set; }

        /// <summary>
        /// Gets or sets the status of the subscriber.
        /// </summary>
        [JsonProperty("status")]
        public SubscriberStatus Status { get; set; }
    }
}