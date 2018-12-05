using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using ServiceStack;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.MailchimpConnector.Client.Lists;
using Telerik.Sitefinity.MailchimpConnector.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Model;
using Telerik.Sitefinity.MailchimpConnector.Web.Services.DTO;
using Telerik.Sitefinity.Services.ServiceStack.Filters;

namespace Telerik.Sitefinity.MailchimpConnector.Web.Services
{
    /// <summary>
    /// Class that represents the service stack service.
    /// </summary>
    public class MailchimpWebService : Service
    {
        /// <summary>
        /// Method that returns suggestions for mailchimp lists.
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>Returns suggestions for mailchimp lists.</returns>
        [AddHeader(ContentType = "application/json")]
        [RequestBackendAuthenticationFilterAttribute]
        public string[] Get(MailchimpListRequest request)
        {
            string[] result = new string[] { };
            var config = Config.Get<MailchimpConnectorConfig>();
            
            if (config.Enabled)
            {
                IEnumerable<MailchimpList> lists = this.MailchimpListProvider.GetLists();
                if (lists != null)
                {
                    if (!string.IsNullOrEmpty(request.Term))
                    {
                        lists = lists.Where(l => l.Name.IndexOf(request.Term, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    result = lists.Select(f => f.Name).Take(request.Take).ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Method that saves Mailchimp configuration.
        /// </summary>
        /// <param name="request">Request object</param>
        [AddHeader(ContentType = "application/json")]
        [RequestAdministrationAuthenticationFilter]
        public void Post(MailchimpConfigurationRequest request)
        {
            try
            {
                MailchimpConnectorConfig mailchimpConnectorConfig = new MailchimpConnectorConfig();
                mailchimpConnectorConfig.MailchimpApiKey = request.ApiKey;

                using (var testConnectionClient = new MailchimpListClient(mailchimpConnectorConfig, new HttpClient()))
                {
                    testConnectionClient.GetLists();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, TraceEventType.Error);

                throw new Exception(Res.Get<Labels>().UnableToConnectCheckYourCredentials);
            }

            var configManager = ConfigManager.GetManager();
            var config = configManager.GetSection<MailchimpConnectorConfig>();

            config.Enabled = true;
            config.MailchimpApiKey = request.ApiKey;

            configManager.SaveSection(config, true);
        }

        /// <summary>
        /// Changes the state of Mailchimp module - enabled/disabled.
        /// </summary>
        /// <param name="request">The state</param>
        /// <returns>The module state</returns>
        [AddHeader(ContentType = "application/json")]
        [RequestAdministrationAuthenticationFilter]
        public MailchimpStatusRequest Post(MailchimpStatusRequest request)
        {
            var configManager = ConfigManager.GetManager();
            var config = configManager.GetSection<MailchimpConnectorConfig>();

            config.Enabled = request.Enabled;

            configManager.SaveSection(config, true);

            return request;
        }

        internal IMailchimpListProvider MailchimpListProvider
        {
            get
            {
                if (this.mailchimpListProvider == null)
                {
                    this.mailchimpListProvider = ObjectFactory.Resolve<IMailchimpListCache>();
                }

                return this.mailchimpListProvider;
            }
        }

        private IMailchimpListProvider mailchimpListProvider;
    }
}