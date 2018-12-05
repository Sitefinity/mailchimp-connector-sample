using System.Collections.Generic;
using Newtonsoft.Json;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.MailchimpConnector.Client.Lists.DTO
{
    internal class GetMergeFieldsResult
    {
        /// <summary>
        /// Gets or sets the enumeration of merge fields.
        /// </summary>
        [JsonProperty("merge_fields")]
        public IEnumerable<MailchimpListMergeField> MergeFields { get; set; }
    }
}
