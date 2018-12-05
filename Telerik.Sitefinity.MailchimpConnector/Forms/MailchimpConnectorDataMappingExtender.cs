using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Client.Lists;
using Telerik.Sitefinity.MailchimpConnector.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Constants;
using Telerik.Sitefinity.MailchimpConnector.Model;
using Telerik.Sitefinity.Modules.Forms;

namespace Telerik.Sitefinity.MailchimpConnector.Forms
{
    /// <summary>
    /// The class defines data mapping between Sitefinity and Mailchimp connectors' fields.
    /// </summary>
    internal class MailchimpConnectorDataMappingExtender : ConnectorDataMappingExtender
    {
        /// <inheritdoc />
        public override string Key
        {
            get 
            {
                return MailchimpConnectorModule.ConnectorName;
            }
        }

        /// <inheritdoc />
        public override string DependentControlsCssClass
        {
            get
            {
                return MailchimpFormsConnectorDefinitionsExtender.DependentControlsCssClass;
            }
        }

        /// <inheritdoc />
        public override string AutocompleteRequiredControlsCssClass
        {
            get
            {
                return MailchimpFormsConnectorDefinitionsExtender.DependentControlsCssClass;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpConnectorDataMappingExtender"/> class.
        /// </summary>
        public MailchimpConnectorDataMappingExtender()
            : this(ObjectFactory.Resolve<IMailchimpListCache>(), Config.Get<MailchimpConnectorConfig>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpConnectorDataMappingExtender"/> class.
        /// </summary>
        /// <param name="mailchimpFormsProvider">The <see cref="IMailchimpListProvider"/> instance that will be used in the class.</param>
        /// <param name="mailchimpConnectorConfig">The <see cref="MailchimpConnectorConfig"/> instance that will be used in the class.</param>
        internal MailchimpConnectorDataMappingExtender(IMailchimpListProvider mailchimpListProvider, MailchimpConnectorConfig mailchimpConnectorConfig)
        {
            this.mailchimpListProvider = mailchimpListProvider;
            this.mailchimpConnectorConfig = mailchimpConnectorConfig;
        }

        /// <inheritdoc />
        public override IEnumerable<string> GetAutocompleteData(string term, string[] paramValues)
        {
            if (!this.mailchimpConnectorConfig.Enabled || !paramValues.Any())
            {
                return null;
            }

            string listName = paramValues[0];
            if (string.IsNullOrWhiteSpace(listName))
            {
                return null;
            }

            IEnumerable<MailchimpList> lists = this.mailchimpListProvider.GetLists();
            if (lists == null || !lists.Any())
            {
                return null;
            }

            MailchimpList list = lists.FirstOrDefault(f => f.Name == listName);
            if (list == null)
            {
                return null;
            }

            IEnumerable<MailchimpListMergeField> mergeFields = this.mailchimpListProvider.GetMergeFields(list.Id);

            IEnumerable<string> result = new List<string>();
            if (mergeFields != null && mergeFields.Any())
            {
                result = mergeFields.Select(mf => mf.Name);
            }

            result = result.Append(FieldNameConstants.Email);

            if (!string.IsNullOrWhiteSpace(term))
            {
                result = result.Where(f => f != null && f.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            int take = this.mailchimpConnectorConfig.AutocompleteSuggestionsCount;
            if (take > 0)
            {
                result = result.Take(take);
            }

            return result;
        }

        /// <inheritdoc />
        public override bool HasAutocomplete
        {
            get
            {
                return true;
            }
        }

        private readonly IMailchimpListProvider mailchimpListProvider;
        private readonly MailchimpConnectorConfig mailchimpConnectorConfig;
    }
}