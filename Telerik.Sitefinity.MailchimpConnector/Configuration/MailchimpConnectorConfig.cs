using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace Telerik.Sitefinity.MailchimpConnector.Configuration
{
    /// <summary>
    /// Configuration file for MailchimpConnector
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MailchimpConnectorConfig : ConfigSection
    {
        #region Properties

        /// <summary>
        /// Gets or sets Mailchimp API key
        /// </summary>
        [ObjectInfo(typeof(MailchimpConnectorResources), Title = "MailchimpApiKey", Description = "MailchimpApiKey")]
        [ConfigurationProperty("MailchimpApiKey")]
        public string MailchimpApiKey
        {
            get
            {
                return (string)this["MailchimpApiKey"];
            }

            set
            {
                this["MailchimpApiKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connector is enabled
        /// </summary>
        [ObjectInfo(typeof(MailchimpConnectorResources), Title = "Enabled", Description = "Enabled")]
        [ConfigurationProperty("Enabled")]
        [Browsable(false)]
        public bool Enabled
        {
            get
            {
                return (bool)this["Enabled"];
            }

            set
            {
                this["Enabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the number of items to return when requesting auto-complete data.
        /// </summary>
        [ObjectInfo(typeof(MailchimpConnectorResources), Title = "AutocompleteSuggestionsCount", Description = "AutocompleteSuggestionsCount")]
        [ConfigurationProperty("AutocompleteSuggestionsCount", DefaultValue = 7)]
        public int AutocompleteSuggestionsCount
        {
            get
            {
                return (int)this["AutocompleteSuggestionsCount"];
            }

            set
            {
                this["AutocompleteSuggestionsCount"] = value;
            }
        }
        
        #endregion
    }
}