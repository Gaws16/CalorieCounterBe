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
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty.", nameof(message));
            }

            // Explane to Gemini what it should do
            var initialRequset = Environment.GetEnvironmentVariable("GEMINI_INITIAL_PROMPT")
                ?? throw new ApplicationException("GEMINI_INITIAL_PROMPT environment variable is not set.");

            var returnFormat = Environment.GetEnvironmentVariable("GEMINI_REESPONSE_FORMAT")
                ?? throw new ApplicationException("GEMINI_REESPONSE_FORMAT environment variable is not set.");


            var requestBody = new
            {
                contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = initialRequset }
                    },
                },
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = message }
                    }
                },
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = returnFormat }
                    }
                }



            }
            };

            var response = await httpClient.PostAsJsonAsync("models/gemini-1.5-pro:generateContent", requestBody);
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
