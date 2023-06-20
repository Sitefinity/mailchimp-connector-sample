using System;
using Telerik.Sitefinity.Services;

namespace Telerik.Sitefinity.MailchimpConnector
{
    internal class MailchimpConnectionStatus : IModuleConnectionStatus
    {
        public string ModuleName => MailchimpConnectorModule.ModuleName;

        public void ExecuteIfConfigured(Action action)
        {
            if (MailchimpConnectorModule.MailchimpConfigHasRequiredSettings() && action != null)
            {
                action();
            }
        }

        public void ExecuteIfNotConfigured(Action action)
        {
            if (!MailchimpConnectorModule.MailchimpConfigHasRequiredSettings() && action != null)
            {
                action();
            }
        }
    }
}
