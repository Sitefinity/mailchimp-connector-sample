using System.Globalization;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Web.Services;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields.Config;
using Telerik.Sitefinity.Web.UI.Fields.Enums;

namespace Telerik.Sitefinity.MailchimpConnector.Forms
{
    /// <summary>
    /// Extends forms definitions by adding Mailchimp specific fields in the form properties.
    /// </summary>
    internal class MailchimpFormsConnectorDefinitionsExtender : FormsConnectorDefinitionsExtender
    {
        /// <inheritdoc/>
        public override int Ordinal
        {
            get
            {
                return 3;
            }
        }

        /// <inheritdoc/>
        public override void AddConnectorSettings(ConfigElementDictionary<string, FieldDefinitionElement> sectionFields)
        {
            var mailchimpListNameField = new TextFieldDefinitionElement(sectionFields)
            {   
                Title = MailchimpFormsConnectorDefinitionsExtender.MailchimpListNameFieldName,
                DataFieldName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", FormAttributesPropertyName, MailchimpFormsConnectorDefinitionsExtender.MailchimpListNameFieldName),
                DisplayMode = FieldDisplayMode.Write,
                FieldName = MailchimpFormsConnectorDefinitionsExtender.MailchimpListNameFieldName,
                CssClass = MailchimpFormsConnectorDefinitionsExtender.DependentControlsCssClass,
                FieldType = typeof(TextField),
                ID = MailchimpFormsConnectorDefinitionsExtender.MailchimpListNameFieldName,
                ResourceClassId = ResourceClassId,
                AutocompleteServiceUrl = string.Concat("/restapi", MailchimpServiceStackPlugin.ListsRoute, "?take={take}"),
                AutocompleteSuggestionsCount = Config.Get<MailchimpConnectorConfig>().AutocompleteSuggestionsCount
            };

            sectionFields.Add(mailchimpListNameField);
        }

        private const string FormAttributesPropertyName = "Attributes";
        public const string MailchimpListNameFieldName = "MailchimpListName";
        public const string DependentControlsCssClass = "sfMailchimpDependentCtrls";
        public static readonly string ResourceClassId = typeof(MailchimpConnectorResources).Name;
    }
}