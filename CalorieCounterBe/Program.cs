using CalorieCounterBe.Core.Contracts;
using CalorieCounterBe.Core.Services;
using CalorieCounterBe.Handlers;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure Supabase client
var supabaseUrl = builder.Configuration.GetSection("Supabase:Url").Value
    ?? throw new ApplicationException("Supabase URL is not configured in appsettings.json or environment variables.");

var supabaseKey = builder.Configuration.GetSection("Supabase:Key").Value
    ?? throw new ApplicationException("Supabase Key is not configured in appsettings.json or environment variables.");

var suppabaseOptions = new Supabase.SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = true,
};
// Configure OpenAI client
builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/");

    var openAiKey = builder.Configuration.GetSection("OpenAI:Key").Value
        ?? throw new ApplicationException("OpenAI Key is not configured in appsettings.json or environment variables.");

    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer",openAiKey);
});

// Configure Gemini client
builder.Services.AddHttpClient("Gemini", client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/");
    
  
}).AddHttpMessageHandler<GeminiAuthHandler>();

// Register services
builder.Services.AddSingleton(provider => new Supabase.Client(supabaseUrl, supabaseKey, suppabaseOptions));
// Register Gemini authentication handler
builder.Services.AddTransient<GeminiAuthHandler>();
builder.Services.AddScoped<CalorieCounterBe.Core.Services.SupabaseService>();
builder.Services.AddScoped<IGptService, GptService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
