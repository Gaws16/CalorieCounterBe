using System.Net.Http.Headers;
using System.Web;

namespace CalorieCounterBe.Handlers
{
    public class GeminiAuthHandler : DelegatingHandler
    {
        private readonly IConfiguration configuration;

        public GeminiAuthHandler(IConfiguration config)
        {
            configuration = config;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiKey = configuration.GetSection("Gemini:Key").Value
                ?? throw new ApplicationException("Gemini Key is not configured in appsettings.json or environment variables.");
           var uri = request.RequestUri
                     ?? throw new ApplicationException("Request URI is null.");

           // if (uri.Scheme != "https" || uri.Host != "generativelanguage.googleapis.com")
             //   throw new ApplicationException($"Blocked request to untrusted host: {uri.Host}");

            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["key"] = apiKey; // Add the API key to the query string
            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri; // Update the request URI with the new query string


            return await base.SendAsync(request, cancellationToken);
        }
    }
}
