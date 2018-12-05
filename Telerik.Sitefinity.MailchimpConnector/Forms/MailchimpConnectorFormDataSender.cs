using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.MailchimpConnector.Client.Lists;
using Telerik.Sitefinity.MailchimpConnector.Constants;
using Telerik.Sitefinity.MailchimpConnector.Enums;
using Telerik.Sitefinity.MailchimpConnector.Model;
using Telerik.Sitefinity.Modules.Forms;

namespace Telerik.Sitefinity.MailchimpConnector.Forms
{
    /// <summary>
    /// Used for sending form data for the Mailchimp connector integration.
    /// </summary>
    internal class MailchimpConnectorFormDataSender : IConnectorFormDataSender
    {
        /// <inheritdoc/>
        public string DataMappingExtenderKey
        {
            get
            {
                return MailchimpConnectorModule.ConnectorName;
            }
        }

        /// <inheritdoc/>
        public string DesignerExtenderName
        {
            get
            {
                return MailchimpConnectorModule.ConnectorName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpConnectorFormDataSender"/> class.
        /// </summary>
        public MailchimpConnectorFormDataSender()
            : this(ObjectFactory.Resolve<IMailchimpListClient>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpConnectorFormDataSender"/> class.
        /// </summary>
        /// <param name="mailchimpFormDataSubmitter">The <see cref="IMailchimpFormDataSubmitter"/> instance that will be used in the class.</param>
        /// <param name="mailchimpListsClient">The <see cref="IMailchimpListClient"/> instance that will be used in the class.</param>
        internal MailchimpConnectorFormDataSender(IMailchimpListClient mailchimpListsClient)
        {
            if (mailchimpListsClient == null)
            {
                throw new ArgumentNullException("mailchimpListsClient");
            }

            this.mailchimpListsClient = mailchimpListsClient;
        }

        /// <inheritdoc/>
        public bool ShouldSendFormData(ConnectorFormDataContext dataContext)
        {
            bool shouldPostDataToMailchimp = bool.Parse(dataContext.WidgetDesignerSettings[MailchimpFormsConnectorDesignerExtender.PostDataToMailchimpPropertyName]);
            if (!shouldPostDataToMailchimp)
            {
                return false;
            }

            string formName = dataContext.FormDescriptionAttributeSettings[MailchimpFormsConnectorDefinitionsExtender.MailchimpListNameFieldName];
            if (string.IsNullOrWhiteSpace(formName))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public void SendFormData(IDictionary<string, string> data, ConnectorFormDataContext dataContext)
        {
            try
            {
                string listId = this.GetListId(dataContext);

                string email = data[FieldNameConstants.Email];
                data.Remove(FieldNameConstants.Email);

                data = this.ReplaceKeysWithMergeFieldTags(listId, data);

                MailchimpListMember mailchimpListMember = new MailchimpListMember();
                mailchimpListMember.Email = email;
                mailchimpListMember.MergeFields = data;
                mailchimpListMember.Status = SubscriberStatus.Subscribed;

                this.mailchimpListsClient.CreateMember(listId, mailchimpListMember);
            }
            catch (Exception ex)
            {
                Log.Write(ex, TraceEventType.Error);
            }
        }

        /// <summary>
        /// Gets the Mailchimp list id from the provided <see cref="ConnectorFormDataContext"/>.
        /// </summary>
        /// <param name="dataContext">The data context around the submitted form fields.</param>
        /// <returns>The Mailchimp list id.</returns>
        protected virtual string GetListId(ConnectorFormDataContext dataContext)
        {
            if (dataContext == null)
            {
                throw new ArgumentNullException("dataContext");
            }

            string formFieldName = dataContext.FormDescriptionAttributeSettings[MailchimpFormsConnectorDefinitionsExtender.MailchimpListNameFieldName];
            MailchimpList list = this.mailchimpListsClient.GetLists().FirstOrDefault(f => f.Name == formFieldName);

            return list.Id;
        }

        /// <summary>
        /// Gets the mapped submitted form fields where keys have been replaced with merge field tags instead of names. 
        /// </summary>
        /// <param name="listId">The Id of the list.</param>
        /// <param name="data">The data of submitted form fields.</param>
        /// <returns>The data of submitted form fields where keys have been replaced with merge field tags instead of names.</returns>
        protected virtual IDictionary<string, string> ReplaceKeysWithMergeFieldTags(string listId, IDictionary<string, string> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            IEnumerable<MailchimpListMergeField> mergeFields = this.mailchimpListsClient.GetMergeFields(listId);

            IDictionary<string, string> mappedData = new Dictionary<string, string>();
            foreach (var item in data)
            {
                MailchimpListMergeField mergeField = mergeFields.FirstOrDefault(mf => mf.Name == item.Key);
                if (mergeField != null)
                {
                    mappedData.Add(mergeField.Tag, item.Value);
                }
            }

            return mappedData;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the managed resources
        /// </summary>
        /// <param name="disposing">Defines whether a disposing is executing now.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.mailchimpListsClient != null)
                {
                    this.mailchimpListsClient.Dispose();
                }
            }
        }

        private readonly IMailchimpListClient mailchimpListsClient;
    }
}