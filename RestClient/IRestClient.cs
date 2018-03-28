using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestClient
{
    public interface IRestClient
    {
        Task Send(string url, HttpMethod method, Func<object> getContent = null);

        Task<T> Send<T>(string url, HttpMethod method, Func<object> getContent = null, Func<HttpResponseMessage, T> resultFunc = null);
    }
}