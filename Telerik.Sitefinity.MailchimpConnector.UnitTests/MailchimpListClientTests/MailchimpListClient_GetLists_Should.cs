using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.MailchimpConnector.Client.Lists;
using Telerik.Sitefinity.MailchimpConnector.Configuration;
using Telerik.Sitefinity.Test.Unit.Mailchimp.MailchimpListsClientTests.Mocks;

namespace Telerik.Sitefinity.Test.Unit.Mailchimp.MailchimpListClientTests
{
    /// <summary>
    /// Mailchimp form client GetLists unit tests.
    /// </summary>
    [TestClass]
    public class MailchimpListClient_GetLists_Should
    {
        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns server error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsServerError()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            };

            // Act
            MailchimpListClient client = new MailchimpListClient(new MailchimpConnectorConfig() { MailchimpApiKey = "someApiKey-dc" }, new HttpClient(httpMessageHandlerMock));
            client.GetLists();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns unauthorized.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsUnauthorized()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            };

            // Act
            MailchimpListClient client = new MailchimpListClient(new MailchimpConnectorConfig() { MailchimpApiKey = "someApiKey-dc" }, new HttpClient(httpMessageHandlerMock));
            client.GetLists();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns forbidden.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsForbidden()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            };

            // Act
            MailchimpListClient client = new MailchimpListClient(new MailchimpConnectorConfig() { MailchimpApiKey = "someApiKey-dc" }, new HttpClient(httpMessageHandlerMock));
            client.GetLists();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns bad request.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsBadRequest()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            };

            // Act
            MailchimpListClient client = new MailchimpListClient(new MailchimpConnectorConfig() { MailchimpApiKey = "someApiKey-dc" }, new HttpClient(httpMessageHandlerMock));
            client.GetLists();
        }

        /// <summary>
        /// Tests whether the method will throw HttpRequestException when service returns not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void ThrowException_WhenServiceReturnsNotFound()
        {
            // Arrange
            HttpMessageHandlerMock httpMessageHandlerMock = new HttpMessageHandlerMock();
            httpMessageHandlerMock.SendAsyncFunc = (httpRequestMessage) => 
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            };

            // Act
            MailchimpListClient client = new MailchimpListClient(new MailchimpConnectorConfig() { MailchimpApiKey = "someApiKey-dc" }, new HttpClient(httpMessageHandlerMock));
            client.GetLists();
        }
    }
}