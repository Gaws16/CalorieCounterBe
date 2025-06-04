using CalorieCounterBe.Core.Contracts;
using CalorieCounterBe.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalorieCounterBe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SupabaseService supabaseService;
        private readonly IGptService gptService;
        private readonly IGeminiService geminiService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, SupabaseService supabase, IGptService gptService, IGeminiService geminiService)
        {
            _logger = logger;
            supabaseService = supabase ?? throw new ArgumentNullException(nameof(supabase), "Supabase service cannot be null.");
            this.gptService = gptService;
            this.geminiService = geminiService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("test")]
        public async Task<IActionResult> TestSupabase()
        {
            try
            {
                var users = await supabaseService.GetUsers();
                var mapped = users.Select(t => new
                {
                    id = t.Id,
                    ime = t.Ime

                });
                return Ok(mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("gpt")]
        public async Task<IActionResult> SendMessageToGpt([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }
            try
            {
                var response = await gptService.SendMessageAsync(message);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with GPT service.");
                return StatusCode(500, "Error communicating with GPT service.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpPost("gemini")]
        public async Task<IActionResult> SendMessageToGemini([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }
            try
            {
                var response = await geminiService.SendMessageAsync(message);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with Gemini service.");
                return StatusCode(500, "Error communicating with Gemini service.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
