using System;
using System.Diagnostics.CodeAnalysis;
using ServiceStack;
using Telerik.Sitefinity.MailchimpConnector.Web.Services.DTO;

namespace Telerik.Sitefinity.MailchimpConnector.Web.Services
{
    /// <summary>
    /// Represents a Mailchimp plug-in for the search web service.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class MailchimpServiceStackPlugin : IPlugin
    {
        /// <summary>
        /// Adding the service routes
        /// </summary>
        /// <param name="appHost">The service stack appHost</param>
        public void Register(IAppHost appHost)
        {
            if (appHost == null)
                throw new ArgumentNullException("appHost");

            appHost.RegisterService<MailchimpWebService>();
            appHost.Routes
                   .Add<MailchimpListRequest>(ListsRoute, "GET")
                   .Add<MailchimpConfigurationRequest>(ConfigurationRoute, "POST")
                   .Add<MailchimpStatusRequest>(ModuleStatusRoute, "POST");
        }

        internal static readonly string ListsRoute = string.Concat(ServiceRoute, "/lists");
        internal static readonly string ConfigurationRoute = string.Concat(ServiceRoute, "/config");
        internal static readonly string ModuleStatusRoute = string.Concat(ServiceRoute, "/status");
        private const string ServiceRoute = "/mailchimp";
    }
}