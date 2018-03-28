using System.Net.Http;
using System.Net.Http.Formatting;

namespace RestClient
{
    internal static class HttpMessageExtensions
    {
        public static HttpRequestMessage WithContent<T>(this HttpRequestMessage message, T content)
        {
            message.Content = GetHttpContent<T>(content);
            return message;
        }

        private static HttpContent GetHttpContent<T>(T content)
        {
            var httpContent = content as HttpContent;
            if (httpContent != null)
            {
                return httpContent;
            }
            return new ObjectContent<T>(content, new JsonMediaTypeFormatter());
        }
    }
}