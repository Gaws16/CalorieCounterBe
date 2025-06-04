using CalorieCounterBe.Core.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace CalorieCounterBe.Core.Services
{
    public class GptService : IGptService
    {
        private readonly HttpClient httpClient;

        public GptService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("OpenAI");
        }

        public Task<string> GetStatusAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendMessageAsync(string message)
        {
            var requst = new
            {
                model = "gpt-3.5-turbo" ,
                Messages = new[]
                {
                        new { Role = "user", Content = message }
                    },
                MaxTokens = 1000
            };

            var response = await httpClient.PostAsJsonAsync("chat/completions", requst);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                var content = (string?)result?.Choices?[0]?.Message?.Content;

                return content ?? "Something went wrong with the response!";
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
