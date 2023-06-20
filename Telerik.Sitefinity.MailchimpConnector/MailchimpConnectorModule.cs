using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Abstractions.VirtualPath.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Connectivity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.MailchimpConnector.Client.Lists;
using Telerik.Sitefinity.MailchimpConnector.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Forms;
using Telerik.Sitefinity.MailchimpConnector.Web.Services;
using Telerik.Sitefinity.MailchimpConnector.Web.UI;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Services;

namespace Telerik.Sitefinity.MailchimpConnector
{
    /// <summary>
    /// Mailchimp connector module
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MailchimpConnectorModule : ModuleBase
    {
        /// <inheritdoc />
        public override Guid LandingPageId
        {
            get
            {
                return PageId;
            }
        }

        /// <inheritdoc />
        public override void Initialize(ModuleSettings settings)
        {
            App.WorkWith()
                .Module(MailchimpConnectorModule.ModuleName)
                .Initialize()
                .Localization<MailchimpConnectorResources>()
                .Configuration<MailchimpConnectorConfig>()
                .ServiceStackPlugin(new MailchimpServiceStackPlugin());

            base.Initialize(settings);
            this.RegisterIocTypes();
        }

        /// <summary>
        /// Integrate the module into the system.
        /// </summary>
        public override void Load()
        {
            Bootstrapper.Initialized -= this.Bootstrapper_Initialized;
            Bootstrapper.Initialized += this.Bootstrapper_Initialized;
        }

        /// <summary>
        /// Handles the Initialized event of the Bootstrapper.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Sitefinity.Data.ExecutedEventArgs"/> instance containing the event data.</param>
        protected virtual void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped" && SystemManager.GetModule(MailchimpConnectorModule.ModuleName) != null)
            {
                var configManager = ConfigManager.GetManager();
                configManager.Provider.Executed += this.ConfigEventHandler;

                if (MailchimpConfigHasRequiredSettings())
                {
                    this.InitializeFormDataSender();
                    this.InitializeMailchimpFormsCache();
                }
            }
        }

        /// <summary>
        /// This method is invoked during the unload process of an active module from Sitefinity, e.g. when a module is deactivated. For instance this method is also invoked for every active module during a restart of the application. 
        /// Typically you will use this method to unsubscribe the module from all events to which it has subscription.
        /// </summary>
        public override void Unload()
        {
            this.DisposeFormDataSender();
            this.DisposeSingletonInstances();

            var configManager = ConfigManager.GetManager();
            configManager.Provider.Executed -= this.ConfigEventHandler;

            Bootstrapper.Initialized -= this.Bootstrapper_Initialized;

            base.Unload();
        }

        /// <summary>
        /// Uninstall the module from Sitefinity system. Deletes the module artifacts added with Install method.
        /// </summary>
        /// <param name="initializer">The site initializer instance.</param>
        public override void Uninstall(SiteInitializer initializer)
        {
            this.DisposeFormDataSender();
            this.DisposeSingletonInstances();

            var configManager = ConfigManager.GetManager();
            configManager.Provider.Executed -= this.ConfigEventHandler;

            base.Uninstall(initializer);
        }

        /// <inheritdoc />
        public override void Install(Abstractions.SiteInitializer initializer)
        {
            ConnectorsHelper.CreateConnectivityGroupPage(initializer);

            initializer.Installer
                .CreateModuleGroupPage(ModuleGroupPageId, "MailchimpConnectorGroupPage")
                    .PlaceUnder(SiteInitializer.ConnectivityPageNodeId)
                    .SetOrdinal(4.7f)
                    .LocalizeUsing<MailchimpConnectorResources>()
                    .SetTitleLocalized("MailchimpConnectorGroupPageTitle")
                    .SetUrlNameLocalized("MailchimpConnectorGroupPageUrlName")
                    .SetDescriptionLocalized("MailchimpConnectorGroupPageDescription")
                    .ShowInNavigation()
                    .AddChildPage(PageId, "MailchimpConnectorPage")
                        .SetOrdinal(1)
                        .LocalizeUsing<MailchimpConnectorResources>()
                        .SetTitleLocalized("MailchimpConnectorPageTitle")
                        .SetHtmlTitleLocalized("MailchimpConnectorPageTitle")
                        .SetUrlNameLocalized("MailchimpConnectorPageUrlName")
                        .SetDescriptionLocalized("MailchimpConnectorPageDescription")
                        .AddControl(new MailchimpConnectorSettings())
                        .HideFromNavigation()
                    .Done()
                .Done();
        }

        /// <inheritdoc />
        protected override ConfigSection GetModuleConfig()
        {
            return Config.Get<MailchimpConnectorConfig>();
        }

        /// <inheritdoc />
        protected override IDictionary<string, Action<VirtualPathElement>> GetVirtualPaths()
        {
            var paths = new Dictionary<string, Action<VirtualPathElement>>();
            paths.Add(ModuleVirtualPath + "*", null);
            return paths;
        }

        /// <inheritdoc />
        public override Type[] Managers
        {
            get
            {
                return new Type[0];
            }
        }

        /// <summary>
        /// Handles the event for config update.
        /// </summary>
        /// <param name="configEvent">The config change event args.</param>
        private void ConfigEventHandler(object sender, ExecutedEventArgs e)
        {
            bool isMailchimpConfigUpdated = e.CommandArguments is MailchimpConnectorConfig;

            if (!isMailchimpConfigUpdated)
            {
                return;
            }

            this.DisposeFormDataSender();
            this.DisposeSingletonInstances();

            if (MailchimpConfigHasRequiredSettings())
            {
                this.InitializeFormDataSender();
                this.InitializeMailchimpFormsCache();
            }
        }

        /// <summary>
        /// Initializes the local <see cref="MailchimpConnectorFormDataSender"/>.
        /// </summary>
        private void InitializeFormDataSender()
        {
            this.connectorFormDataSender = new MailchimpConnectorFormDataSender();
            ConnectorFormsEventHandler.RegisterSender(this.connectorFormDataSender);
        }

        /// <summary>
        /// Initializes the forms cache and re initializes the IMailchimpFormsClient
        /// </summary>
        private void InitializeMailchimpFormsCache()
        {
            SystemManager.BackgroundTasksService.EnqueueTask(() =>
            {
                IMailchimpListCache mailchimpListCache = ObjectFactory.Resolve<IMailchimpListCache>();
                mailchimpListCache.GetLists();
            });
        }

        /// <summary>
        /// Disposes the local <see cref="MailchimpConnectorFormDataSender"/>.
        /// </summary>
        private void DisposeFormDataSender()
        {
            if (this.connectorFormDataSender != null)
            {
                ConnectorFormsEventHandler.UnregisterSender(this.connectorFormDataSender);
                this.connectorFormDataSender.Dispose();
                this.connectorFormDataSender = null;
            }
        }

        /// <summary>
        /// Disposes all singleton instances that have <see cref="ContainerControlledLifetimeManager"/> registered in the local
        /// containerControlledLifetimeManagers field.
        /// </summary>
        private void DisposeSingletonInstances()
        {
            foreach (ContainerControlledLifetimeManager containerControlledLifetimeManager in this.containerControlledLifetimeManagers)
            {
                containerControlledLifetimeManager.RemoveValue();
            }
        }

        /// <summary>
        /// Checks whether the Mailchimp config has the required settings for the connector to work.
        /// </summary>
        /// <returns>Returns true if the config has the required settings for the connector. Otherwise, false.</returns>
        internal static bool MailchimpConfigHasRequiredSettings()
        {
            MailchimpConnectorConfig mailchimpConnectorConfig = Config.Get<MailchimpConnectorConfig>();

            return MailchimpConfigHasRequiredSettings(mailchimpConnectorConfig);
        }

        /// <summary>
        /// Checks whether the Mailchimp config has the required settings for the connector to work.
        /// </summary>
        /// <param name="mailchimpConnectorConfig">The Mailchimp config object.</param>
        /// <returns>Returns true if the config has the required settings for the connector. Otherwise, false.</returns>
        private static bool MailchimpConfigHasRequiredSettings(MailchimpConnectorConfig mailchimpConnectorConfig)
        {
            if (string.IsNullOrWhiteSpace(mailchimpConnectorConfig.MailchimpApiKey))
            {
                return false;
            }

            if (!mailchimpConnectorConfig.Enabled)
            {
                return false;
            }

            return true;
        }

        private void RegisterIocTypes()
        {
            ObjectFactory.Container.RegisterType<FormsConnectorDesignerExtender, MailchimpFormsConnectorDesignerExtender>(MailchimpConnectorModule.ModuleName);
            ObjectFactory.Container.RegisterType<FormsConnectorDefinitionsExtender, MailchimpFormsConnectorDefinitionsExtender>(MailchimpConnectorModule.ModuleName);
            ObjectFactory.Container.RegisterType<ConnectorDataMappingExtender, MailchimpConnectorDataMappingExtender>(MailchimpConnectorModule.ModuleName);

            ContainerControlledLifetimeManager mailchimpListClientLifetimeManager = new ContainerControlledLifetimeManager();
            ObjectFactory.Container.RegisterType<IMailchimpListClient, MailchimpListClient>(mailchimpListClientLifetimeManager);
            this.containerControlledLifetimeManagers.Add(mailchimpListClientLifetimeManager);

            ContainerControlledLifetimeManager mailchimpListCacheLifetimeManager = new ContainerControlledLifetimeManager();
            ObjectFactory.Container.RegisterType<IMailchimpListCache, MailchimpListCache>(mailchimpListCacheLifetimeManager);
            this.containerControlledLifetimeManagers.Add(mailchimpListCacheLifetimeManager);

            ObjectFactory.Container.RegisterType<IModuleConnectionStatus, MailchimpConnectionStatus>(typeof(MailchimpConnectionStatus).FullName, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// The name of this module
        /// </summary>
        public const string ModuleName = "MailchimpConnector";

        /// <summary>
        /// The name of the connector.
        /// </summary>
        internal const string ConnectorName = "Mailchimp";

        internal static readonly Guid ModuleGroupPageId = new Guid("430E4567-AE83-473D-BC11-63EC506C1034");
        internal static readonly Guid PageId = new Guid("B4BB079C-100A-4E47-A384-D20996F001FA");

        internal const string ModuleVirtualPath = "~/MailchimpConnector/";
        private const string MailchimpConnectorConfigName = "MailchimpConnectorConfig";

        private MailchimpConnectorFormDataSender connectorFormDataSender;
        private IList<ContainerControlledLifetimeManager> containerControlledLifetimeManagers = new List<ContainerControlledLifetimeManager>();
    }
}