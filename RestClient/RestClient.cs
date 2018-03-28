using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestClient
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _client;

        public RestClient(HttpClientHandler handler)
        {
            _client = new HttpClient(handler);
        }

        private async Task<T> SendInternal<T>(string url, HttpMethod method, Func<object> getContentDelegate, Func<HttpResponseMessage, T> resultFunc = null)
        {
            var responce = await SendCore(() =>
            {
                var requestMessage = new HttpRequestMessage(method, url);

                if (getContentDelegate != null)
                {
                    requestMessage.WithContent(getContentDelegate.Invoke());
                }

                return requestMessage;
            });

            await EnshureResponce(responce);

            if (resultFunc!=null)
            {
                return resultFunc.Invoke(responce);
            }

            return await responce.Content.ReadAsAsync<T>().ConfigureAwait(false);
        }

        private async Task SendInternal(string url, HttpMethod method, Func<object> getContentDelegate)
        {
            var responce = await SendCore(() =>
            {
                var requestMessage = new HttpRequestMessage(method, url);

                if (getContentDelegate != null)
                {
                    requestMessage.WithContent(getContentDelegate.Invoke());
                }

                return requestMessage;
            });

            await EnshureResponce(responce);
        }

        private async Task EnshureResponce(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new RestException(
                response.RequestMessage.RequestUri,
                response.RequestMessage.Method,
                response.StatusCode,
                response.ReasonPhrase,
                await response.Content.ReadAsStringAsync()
            );
        }

        protected virtual async Task<HttpResponseMessage> SendCore(Func<HttpRequestMessage> requestDelegate)
        {
            var requestMessage = requestDelegate.Invoke();

            var result = await _client.SendAsync(requestMessage).ConfigureAwait(false);

            return result;
        }


        public async Task Send(string url, HttpMethod method, Func<object> getContent = null)
        {
            await SendInternal(url, method, getContent);
        }

        public async Task<T> Send<T>(string url, HttpMethod method, Func<object> getContent = null, Func<HttpResponseMessage, T> resultFunc = null)
        {
            return await SendInternal(url, method, getContent, resultFunc);
        }
    }
}