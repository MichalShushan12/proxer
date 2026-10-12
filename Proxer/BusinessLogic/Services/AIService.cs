using BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AIService(IConfiguration config)
        {
            _httpClient = new HttpClient();

            _apiKey = config["Gemini:ApiKey"]
                ?? throw new InvalidOperationException("Missing Gemini:ApiKey");
        }

        public async Task<string> RequestRefactorAsync(string originalCode, string actionType, string languageCode)
        {
            string language = ResolveLanguageName(languageCode);

            string prompt = actionType == "ExplainCode"
                ? $"Explain the following code clearly and concisely. " +
                  $"Write your ENTIRE response (titles, explanations, markdown text) in {language}. " +
                  $"Keep code identifiers and keywords unchanged.\n\n{originalCode}"
                : $"Perform '{actionType}' on the following code. " +
                  $"Write all code comments in {language}. " +
                  $"Return ONLY the code, without markdown fences.\n\n{originalCode}";

            var body = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } }
            };

            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash:generateContent")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            // המפתח בכותרת במקום ב-URL, כדי שלא יופיע בלוגים
            request.Headers.Add("x-goog-api-key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            string json = await response.Content.ReadAsStringAsync();

            // שגיאה שמוחזרת למשתמש אם עבר את מכסת ההודעות בגלל שזו גרסה חינמית
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                throw new HttpRequestException("The AI service quota was exceeded. Please try again later.");

            //החזרת שגיאה על כל תקלה אחרת
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Gemini error {response.StatusCode}: {json}");

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()?.Trim() ?? "";
        }

        private static string ResolveLanguageName(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return "English";
            try
            {
                // "he-IL" -> "he" -> "Hebrew"
                return CultureInfo.GetCultureInfo(code.Split('-')[0]).EnglishName;
            }
            catch (CultureNotFoundException)
            {
                return "English";
            }
        }
    }
}
