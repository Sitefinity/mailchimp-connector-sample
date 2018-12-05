using System.Diagnostics.CodeAnalysis;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace Telerik.Sitefinity.MailchimpConnector
{
    /// <summary>
    /// Localizable strings for the MailchimpConnector module
    /// </summary>
    [ExcludeFromCodeCoverage]
    [ObjectInfo("MailchimpConnectorResources", ResourceClassId = "MailchimpConnectorResources", Title = "MailchimpConnectorResourcesTitle", TitlePlural = "MailchimpConnectorResourcesTitlePlural", Description = "MailchimpConnectorResourcesDescription")]
    public class MailchimpConnectorResources : Resource
    {
        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpConnectorResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public MailchimpConnectorResources()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpConnectorResources"/> class with the provided <see cref="ResourceDataProvider"/>.
        /// </summary>
        /// <param name="dataProvider">Data provider <see cref="ResourceDataProvider"/></param>
        public MailchimpConnectorResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }
        #endregion

        #region Class Description
        /// <summary>
        /// Gets MailchimpConnector resources title
        /// </summary>
        /// <value>Mailchimp connector labels</value>
        [ResourceEntry("MailchimpConnectorResourcesTitle",
            Value = "Mailchimp connector labels",
            Description = "The title of this class.",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorResourcesTitle
        {
            get
            {
                return this["MailchimpConnectorResourcesTitle"];
            }
        }

        /// <summary>
        /// Gets MailchimpConnector resources title plural
        /// </summary>
        /// <value>Mailchimp connector labels</value>
        [ResourceEntry("MailchimpConnectorResourcesTitlePlural",
            Value = "Mailchimp connector labels",
            Description = "The title plural of this class.",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorResourcesTitlePlural
        {
            get
            {
                return this["MailchimpConnectorResourcesTitlePlural"];
            }
        }

        /// <summary>
        /// Gets message: Contains localizable resources for MailchimpConnector module.
        /// </summary>
        /// <value>Contains localizable resources for Mailchimp connector module.</value>
        [ResourceEntry("MailchimpConnectorResourcesDescription",
            Value = "Contains localizable resources for Mailchimp connector module.",
            Description = "The description of this class.",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorResourcesDescription
        {
            get
            {
                return this["MailchimpConnectorResourcesDescription"];
            }
        }
        #endregion

        /// <summary>
        /// Gets phrase: Connector for Mailchimp
        /// </summary>
        /// <value>Connector for Mailchimp</value>
        [ResourceEntry("MailchimpConnectorGroupPageTitle",
            Value = "Connector for Mailchimp",
            Description = "Phrase: Connector for Mailchimp",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorGroupPageTitle
        {
            get
            {
                return this["MailchimpConnectorGroupPageTitle"];
            }
        }

        /// <summary>
        /// Gets word: Mailchimp
        /// </summary>
        [ResourceEntry("MailchimpConnectorGroupPageUrlName",
            Value = "Mailchimp",
            Description = "Word: Mailchimp",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorGroupPageUrlName
        {
            get
            {
                return this["MailchimpConnectorGroupPageUrlName"];
            }
        }

        /// <summary>
        /// Gets phrase: Connector for Mailchimp group page
        /// </summary>
        /// <value>Connector for Mailchimp group page</value>
        [ResourceEntry("MailchimpConnectorGroupPageDescription",
            Value = "Connector for Mailchimp group page",
            Description = "Phrase: Connector for Mailchimp group page",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorGroupPageDescription
        {
            get
            {
                return this["MailchimpConnectorGroupPageDescription"];
            }
        }

        /// <summary>
        /// Gets phrase: Connector for Mailchimp
        /// </summary>
        /// <value>Connector for Mailchimp</value>
        [ResourceEntry("MailchimpConnectorPageTitle",
            Value = "Connector for Mailchimp",
            Description = "Phrase: Connector for Mailchimp",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorPageTitle
        {
            get
            {
                return this["MailchimpConnectorPageTitle"];
            }
        }

        /// <summary>
        /// Gets word: Settings
        /// </summary>
        [ResourceEntry("MailchimpConnectorPageUrlName",
            Value = "Settings",
            Description = "Word: Settings",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorPageUrlName
        {
            get
            {
                return this["MailchimpConnectorPageUrlName"];
            }
        }

        /// <summary>
        /// Gets message: Connector for Mailchimp
        /// </summary>
        [ResourceEntry("MailchimpConnectorPageDescription",
            Value = "Connector for Mailchimp",
            Description = "Connector for Mailchimp",
            LastModified = "2018/11/30")]
        public string MailchimpConnectorPageDescription
        {
            get
            {
                return this["MailchimpConnectorPageDescription"];
            }
        }

        /// <summary>
        /// Gets label: Mailchimp form name
        /// </summary>
        /// <value>Mailchimp form name</value>
        [ResourceEntry("MailchimpListName",
            Value = "Mailchimp form name",
            Description = "label: Mailchimp form name",
            LastModified = "2018/11/30")]
        public string MailchimpListName
        {
            get
            {
                return this["MailchimpListName"];
            }
        }

        /// <summary>
        /// Gets label: Mailchimp settings
        /// </summary>
        /// <value>Mailchimp settings</value>
        [ResourceEntry("MailchimpSettings",
            Value = "Mailchimp settings",
            Description = "label: Mailchimp settings",
            LastModified = "2018/11/30")]
        public string MailchimpSettings
        {
            get
            {
                return this["MailchimpSettings"];
            }
        }

        /// <summary>
        /// Gets label: Post data to Mailchimp
        /// </summary>
        /// <value>Post data to Mailchimp</value>
        [ResourceEntry("PostDataToMailchimp",
            Value = "Post data to Mailchimp",
            Description = "label: Post data to Mailchimp",
            LastModified = "2018/11/30")]
        public string PostDataToMailchimp
        {
            get
            {
                return this["PostDataToMailchimp"];
            }
        }

        /// <summary>
        /// Gets message: Form will post data to Mailchimp once enabled in the Forms module. Make sure the form you have selected has the proper Mailchimp settings there.
        /// </summary>
        /// <value>Form will post data to Mailchimp once enabled in the Forms module. Make sure the form you have selected has the proper Mailchimp settings there.</value>
        [ResourceEntry("MailchimpTabFormWidgetInfoText",
            Value = "Form will post data to Mailchimp once enabled in the Forms module. Make sure the form you have selected has the proper Mailchimp settings there.",
            Description = "message: Form will post data to Mailchimp once enabled in the Forms module. Make sure the form you have selected has the proper Mailchimp settings there. ",
            LastModified = "2018/11/30")]
        public string MailchimpTabFormWidgetInfoText
        {
            get
            {
                return this["MailchimpTabFormWidgetInfoText"];
            }
        }

        /// <summary>
        /// Gets text: Connect to Mailchimp using your Mailchimp credentials
        /// </summary>
        /// <value>Connect to Mailchimp using your Mailchimp credentials</value>
        [ResourceEntry("ConnectToMailchimpUsingYourMailchimpCredentials",
            Value = "Connect using your Mailchimp credentials",
            Description = "text: Connect to Mailchimp using your Mailchimp credentials",
            LastModified = "2018/11/30")]
        public string ConnectToMailchimpUsingYourMailchimpCredentials
        {
            get
            {
                return this["ConnectToMailchimpUsingYourMailchimpCredentials"];
            }
        }

        /// <summary>
        /// Gets word: Mailchimp
        /// </summary>
        /// <value>Mailchimp text</value>
        [ResourceEntry("Mailchimp",
            Value = "Mailchimp",
            Description = "word: Mailchimp",
            LastModified = "2018/11/30")]
        public string Mailchimp
        {
            get
            {
                return this["Mailchimp"];
            }
        }

        /// <summary>
        /// Gets label: API key
        /// </summary>
        /// <value>Mailchimp API key</value>
        [ResourceEntry("MailchimpApiKey",
            Value = "API key",
            Description = "Gets label: API key",
            LastModified = "2018/11/30")]
        public string MailchimpApiKey
        {
            get
            {
                return this["MailchimpApiKey"];
            }
        }

        /// <summary>
        /// Gets the API key hint label
        /// </summary>
        /// <value>In your Mailchimp account, click on your <i>account name</i> in the top right corner, then click <i>Account</i>. Click <i>Extras</i> in the menu, then <i>API keys</i>. If a key has never been generated for your account, click <i>Create A Key</i>. Copy the API key to your clipboard. Example value: <i>bd9361b7e44f0bce621c8de8d4def941-us19</i>.</value>
        [ResourceEntry("MailchimpApiKeyHintLabel",
            Value = "In your Mailchimp account, click on your <i>account name</i> in the top right corner, then click <i>Account</i>. Click <i>Extras</i> in the menu, then <i>API keys</i>. If a key has never been generated for your account, click <i>Create A Key</i>. Copy the API key to your clipboard. Example value: <i>bd9361b7e44f0bce621c8de8d4def941-us19</i>.",
            Description = "API key hint label",
            LastModified = "2018/11/26")]
        public string MailchimpApiKeyHintLabel
        {
            get
            {
                return this["MailchimpApiKeyHintLabel"];
            }
        }

        /// <summary>
        /// Gets word: Enabled
        /// </summary>
        /// <value>text - Enabled</value>
        [ResourceEntry("Enabled",
            Value = "Enabled",
            Description = "word: Enabled",
            LastModified = "2018/11/30")]
        public string Enabled
        {
            get
            {
                return this["Enabled"];
            }
        }

        /// <summary>
        /// Gets label: Autocomplete suggestions count
        /// </summary>
        /// <value>Autocomplete suggestions count</value>
        [ResourceEntry("AutocompleteSuggestionsCount",
            Value = "Autocomplete suggestions count",
            Description = "label: Autocomplete suggestions count",
            LastModified = "2018/11/30")]
        public string AutocompleteSuggestionsCount
        {
            get
            {
                return this["AutocompleteSuggestionsCount"];
            }
        }
    }
}