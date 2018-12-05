using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Telerik.Sitefinity.Test.Unit.Mailchimp.MailchimpListsClientTests.Mocks
{
    /// <summary>
    /// Mocked version of the <see cref="System.Net.Http.HttpMessageHandler"/> class
    /// </summary>
    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        /// <summary>
        /// Gets or sets the function that will be executed instead of the SendAsync method. If not set response message with status OK will be used.
        /// </summary>
        public Func<HttpRequestMessage, HttpResponseMessage> SendAsyncFunc { get; set; }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.SendAsyncFunc != null)
            {
               return this.SendAsyncFunc.Invoke(request);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}