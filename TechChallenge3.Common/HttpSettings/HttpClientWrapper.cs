using System.Net;

namespace TechChallenge3.Common.HttpSettings
{
    public sealed class HttpClientWrapper : HttpClient, IHttpClient
    {
        public HttpClientWrapper(int hours, int minutes, int seconds)
        {
            this.Timeout = new TimeSpan(hours, minutes, seconds);
        }

        public async Task<Tuple<string, HttpStatusCode?>> PostAndGetRawResponseAsync(string requestUri, HttpContent content, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    this.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            HttpResponseMessage httpResponse = await this.PostAsync(requestUri, content);

            // Error: Could not communicate with the endpoint.
            if (httpResponse == null)
            {
                return default;
            }

            return new Tuple<string, Nullable<HttpStatusCode>>(
                await httpResponse.Content.ReadAsStringAsync(),
                httpResponse.StatusCode);
        }
    }
}
