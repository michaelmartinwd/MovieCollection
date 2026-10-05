using Microsoft.Extensions.Configuration;
using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;
using System.Text;
using System.Text.Json;

namespace MovieCollectionApplication.Services
{
    public class ClaudeService : IClaudeService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ClaudeService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> AskMovieQuestionAsync(string question)
        {
            var apiKey = _configuration["Anthropic:ApiKey"];
            var model = _configuration["Anthropic:Model"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Anthropic API key is not configured.");
            }

            var request = new ClaudeRequest
            {
                Model = model ?? "claude-haiku-4-5-20251001",
                MaxTokens = 500,

                System = """
                You are a movie assistant for a movie collection application.

                Only answer questions about movies, actors, directors,
                characters, genres, movie history, and cinema.

                If a user asks a question unrelated to movies, politely
                explain that you only answer movie-related questions.

                Keep your answers concise, helpful, and conversational.

                Do not make up movie information. If you are unsure about
                something, say that you are unsure.
                """,

                Messages =
                [
                    new ClaudeMessage
                {
                    Role = "user",
                    Content = question
                }
                ]
            };

            var json = JsonSerializer.Serialize(
                request,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                });

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.anthropic.com/v1/messages");

            httpRequest.Headers.Add("x-api-key", apiKey);
            httpRequest.Headers.Add(
                "anthropic-version",
                "2023-06-01");

            httpRequest.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var responseJson =
                await response.Content.ReadAsStringAsync();

            var claudeResponse =
                JsonSerializer.Deserialize<ClaudeResponse>(
                    responseJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return claudeResponse?
                .Content
                .FirstOrDefault()?
                .Text
                ?? "I couldn't generate an answer.";
        }
    }
}