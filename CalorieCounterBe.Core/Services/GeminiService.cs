using CalorieCounterBe.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalorieCounterBe.Core.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient httpClient;

        public GeminiService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("Gemini");
        }
       

        public async Task<string> SendMessageAsync(string message)
        {
            var requestBody = new
            {
                contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = message }
                    }
                }
            }
            };

            var response = await httpClient.PostAsJsonAsync("models/gemini-1.5-pro-001:generateContent", requestBody);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var result = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

            return result ?? "Something went wrong with the response!";

        }
    }
}
