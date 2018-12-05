using System.Collections.Generic;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Forms;

namespace Telerik.Sitefinity.MailchimpConnector.Forms
{
    /// <summary>
    /// Extends the designer of the Forms widget by adding new settings for Mailchimp. 
    /// </summary>
    internal class MailchimpFormsConnectorDesignerExtender : FormsConnectorDesignerExtender
    {
        /// <inheritdoc/>
        public override string Title
        {
            get
            {
                return MailchimpConnectorModule.ConnectorName;
            }
        }

        /// <inheritdoc/>
        public override string Name
        {
            get
            {
                return MailchimpConnectorModule.ConnectorName;
            }
        }

        /// <inheritdoc/>
        public override IList<PropertyDescription> GetProperties()
        {
            return new List<PropertyDescription>()
            {
                new PropertyDescription()
                {
                    Type = PropertyDescription.PropertyType.Bool,
                    Name = MailchimpFormsConnectorDesignerExtender.PostDataToMailchimpPropertyName,
                    Title = Res.Get<MailchimpConnectorResources>().MailchimpSettings,
                    Text = Res.Get<MailchimpConnectorResources>().PostDataToMailchimp
                }
            };
        }

        public const string PostDataToMailchimpPropertyName = "PostDataToMailchimp";
    }
}