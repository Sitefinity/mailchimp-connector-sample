using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.MailchimpConnector.Configuration;
using Telerik.Sitefinity.Web.UI;

namespace Telerik.Sitefinity.MailchimpConnector.Web.UI
{
    /// <summary>
    /// Mailchimp connector view
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MailchimpConnectorSettings : SimpleScriptView
    {
        /// <inheritdoc />
        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    return MailchimpConnectorSettings.LayoutPath;
                return base.LayoutTemplatePath;
            }

            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        #region Controls

        /// <summary>
        /// Gets the box with user's name.
        /// </summary>
        /// <value>The box.</value>
        protected virtual TextBox ApiKeyTextBox
        {
            get
            {
                return this.Container.GetControl<TextBox>("apiKeyTextBox", true);
            }
        }

        /// <summary>
        /// Gets the connect button.
        /// </summary>
        /// <value>The connect button.</value>
        protected virtual LinkButton ConnectButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("connectButton", true);
            }
        }

        /// <summary>
        /// Gets the change connection button.
        /// </summary>
        /// <value>The button.</value>
        protected virtual LinkButton ChangeConnectionButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("changeConnectionButton", true);
            }
        }

        /// <summary>
        /// Gets the disconnect reconnect button.
        /// </summary>
        /// <value>The button.</value>
        protected virtual LinkButton DisconnectReconnectButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("disconnectReconnectButton", true);
            }
        }

        /// <summary>
        /// Gets the error message control.
        /// </summary>
        /// <value>The control.</value>
        protected virtual HtmlControl ErrorMessageWrapper
        {
            get
            {
                return this.Container.GetControl<HtmlControl>("errorMessageWrapper", true);
            }
        }
        
        /// <summary>
        /// Gets the loading control.
        /// </summary>
        /// <value>The control.</value>
        protected virtual HtmlControl LoadingView
        {
            get
            {
                return this.Container.GetControl<HtmlControl>("loadingView", true);
            }
        }

        #endregion

        /// <inheritdoc />
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var descriptor = new ScriptControlDescriptor(this.GetType().FullName, this.ClientID);

            descriptor.AddProperty("_mailchimpModuleEnabled", Config.Get<MailchimpConnectorConfig>().Enabled);
            descriptor.AddProperty("_connectText", Res.Get<Labels>().Connect);
            descriptor.AddProperty("_disconnectText", Res.Get<Labels>().Disconnect);

            descriptor.AddElementProperty("apiKeyTextBox", this.ApiKeyTextBox.ClientID);

            descriptor.AddElementProperty("connectButton", this.ConnectButton.ClientID);
            descriptor.AddElementProperty("changeConnectionButton", this.ChangeConnectionButton.ClientID);
            descriptor.AddElementProperty("disconnectReconnectButton", this.DisconnectReconnectButton.ClientID);

            descriptor.AddProperty("_errorMessageWrapperId", this.ErrorMessageWrapper.ClientID);
            descriptor.AddProperty("_loadingViewId", this.LoadingView.ClientID);

            return new[] { descriptor };
        }

        /// <inheritdoc />
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            var scripts = new List<ScriptReference>();
            var assemblyName = this.GetType().Assembly.FullName;

            scripts.Add(new ScriptReference("Telerik.Sitefinity.Web.Scripts.ClientManager.js", "Telerik.Sitefinity"));
            scripts.Add(new ScriptReference(MailchimpConnectorSettings.MailchimpConnectorSettingsScript, assemblyName));

            return scripts;
        }

        /// <inheritdoc />
        protected override void InitializeControls(GenericContainer container)
        {
            var config = Config.Get<MailchimpConnectorConfig>();
            this.ApiKeyTextBox.Text = config.MailchimpApiKey;
        }

        private static readonly string LayoutPath = string.Concat(MailchimpConnectorModule.ModuleVirtualPath, "Telerik.Sitefinity.MailchimpConnector.Web.Views.MailchimpConnectorSettings.ascx");
        private const string MailchimpConnectorSettingsScript = "Telerik.Sitefinity.MailchimpConnector.Web.Scripts.MailchimpConnectorSettings.js";
    }
}