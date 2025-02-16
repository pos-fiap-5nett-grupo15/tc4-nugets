using System.Net;

namespace TechChallenge3.Common.HttpSettings
{
    public interface IHttpClient
    {
        Task<Tuple<string, HttpStatusCode?>> PostAndGetRawResponseAsync(
            string requestUri,
            HttpContent content,
            Dictionary<string, string> headers = default);
    }
}
