using System;
using System.Net;
using System.Net.Http;

namespace RestClient
{
    public class RestException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public string ReasonPhrase { get; private set; }

        public string ErrorContent { get; private set; }

        public HttpMethod HttpMethod { get; private set; }

        public Uri Uri { get; private set; }

        public RestException(Uri uri, HttpMethod httpMethod, HttpStatusCode statusCode, string reasonPhrase, string errorContent)
        {
            this.StatusCode = statusCode;
            this.ReasonPhrase = reasonPhrase;
            this.ErrorContent = errorContent;
            this.HttpMethod = httpMethod;
            this.Uri = uri;
        }

    }
}