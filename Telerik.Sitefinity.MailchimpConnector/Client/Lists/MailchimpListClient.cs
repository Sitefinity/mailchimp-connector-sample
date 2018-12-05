using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Client.Lists.DTO;
using Telerik.Sitefinity.MailchimpConnector.Configuration;
using Telerik.Sitefinity.MailchimpConnector.Model;

namespace Telerik.Sitefinity.MailchimpConnector.Client.Lists
{
    /// <summary>
    /// Represents a Mailchimp lists client
    /// </summary>
    internal class MailchimpListClient : IMailchimpListClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpListClient"/> class.
        /// </summary>
        public MailchimpListClient()
            : this(Config.Get<MailchimpConnectorConfig>(), new HttpClient())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailchimpListClient"/> class.
        /// </summary>
        /// <param name="mailchimpConnectorConfig">The Mailchimp connector configuration</param>
        /// <param name="httpClient">The http client used</param>
        /// <param name="formUrlBuilder">The form URL builder</param>
        internal MailchimpListClient(MailchimpConnectorConfig mailchimpConnectorConfig, HttpClient httpClient)
        {
            if (mailchimpConnectorConfig == null)
            {
                throw new ArgumentNullException("mailchimpConnectorConfig");
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException("httpClient");
            }

            this.apiKey = mailchimpConnectorConfig.MailchimpApiKey;
            string apiUrl = this.GetApiUrl(this.apiKey);

            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(apiUrl, UriKind.Absolute);

            string authorizationHeaderValue = this.GetAuthorizationHeaderValue(this.apiKey);

            this.DecorateHeaders(this.httpClient, MailchimpListClient.ApplicationJsonContentType, authorizationHeaderValue);
        }

        /// <inheritdoc/>
        public IEnumerable<MailchimpList> GetLists()
        {
            string url = string.Format(
                CultureInfo.InvariantCulture,
                "/{0}/{1}",
                MailchimpListClient.ApiVersion,
                MailchimpListClient.ListsApiUrlSegment);

            HttpResponseMessage httpResponseMessage = this.httpClient.GetAsync(url).Result;
            httpResponseMessage.EnsureSuccessStatusCode();

            string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            GetListsResult result = JsonConvert.DeserializeObject<GetListsResult>(responseContent);

            return result.Lists;
        }

        /// <inheritdoc/>
        public IEnumerable<MailchimpListMergeField> GetMergeFields(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            string url = string.Format(
                CultureInfo.InvariantCulture,
                "/{0}/{1}/{2}/{3}",
                MailchimpListClient.ApiVersion,
                MailchimpListClient.ListsApiUrlSegment,
                id,
                MailchimpListClient.MergeFieldsApiUrlSegment);

            HttpResponseMessage httpResponseMessage = this.httpClient.GetAsync(url).Result;
            httpResponseMessage.EnsureSuccessStatusCode();

            string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            GetMergeFieldsResult result = JsonConvert.DeserializeObject<GetMergeFieldsResult>(responseContent);

            return result.MergeFields;
        }

        /// <inheritdoc/>
        public MailchimpListMember CreateMember(string id, MailchimpListMember mailchimpListMember)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            if (mailchimpListMember == null)
            {
                throw new ArgumentNullException("mailchimpListMember");
            }

            string url = string.Format(
                CultureInfo.InvariantCulture,
                "/{0}/{1}/{2}/{3}",
                MailchimpListClient.ApiVersion,
                MailchimpListClient.ListsApiUrlSegment,
                id,
                MailchimpListClient.MembersApiUrlSegment);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter(true));

            var mailchimpListJson = JsonConvert.SerializeObject(mailchimpListMember, settings);
            var mailchimpFormHttpContent = new StringContent(mailchimpListJson, Encoding.UTF8, ApplicationJsonContentType);

            HttpResponseMessage httpResponseMessage = this.httpClient.PostAsync(url, mailchimpFormHttpContent).Result;
            httpResponseMessage.EnsureSuccessStatusCode();

            string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            MailchimpListMember mailchimpListMemberResult = JsonConvert.DeserializeObject<MailchimpListMember>(responseContent);

            return mailchimpListMemberResult;
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
                if (this.httpClient != null)
                {
                    this.httpClient.Dispose();
                }
            }
        }

        private string GetApiUrl(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("apiKey");
            }

            string[] apiKeySegments = apiKey.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (apiKeySegments == null || apiKeySegments.Length <= 1)
            {
                throw new Exception("Could not parse API key and extract data center from it.");
            }

            string dataCenter = apiKeySegments[1];
            string apiUrl = string.Format(CultureInfo.InvariantCulture, "https://{0}.api.mailchimp.com/", dataCenter);

            return apiUrl;
        }

        private string GetAuthorizationHeaderValue(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("apiKey");
            }

            string userCredentials = string.Format(CultureInfo.InvariantCulture, "apiKey:{0}", apiKey);
            byte[] userCredentialsBytes = Encoding.UTF8.GetBytes(userCredentials);
            string userCredentialsBase64 = Convert.ToBase64String(userCredentialsBytes);

            string authorizationHeaderValue = string.Format(CultureInfo.InvariantCulture, "Basic {0}", userCredentialsBase64);

            return authorizationHeaderValue;
        }

        private void DecorateHeaders(HttpClient client, string acceptContentTypeHeaderValue, string authorizationHeaderValue)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (string.IsNullOrWhiteSpace(acceptContentTypeHeaderValue))
            {
                throw new ArgumentNullException("acceptContentTypeHeaderValue");
            }

            if (string.IsNullOrWhiteSpace(authorizationHeaderValue))
            {
                throw new ArgumentNullException("authorizationHeaderValue");
            }

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptContentTypeHeaderValue));
            client.DefaultRequestHeaders.Add(MailchimpListClient.AuthorizationHeader, authorizationHeaderValue);
        }

        private readonly HttpClient httpClient;

        private readonly string apiKey;

        private const string ApiVersion = "3.0";
        private const string ListsApiUrlSegment = "lists";
        private const string MergeFieldsApiUrlSegment = "merge-fields";
        private const string MembersApiUrlSegment = "members";
        private const string AuthorizationHeader = "Authorization";
        private const string ApplicationJsonContentType = "application/json";
    }
}