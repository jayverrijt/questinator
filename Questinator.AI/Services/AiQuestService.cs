using System.Text.Json;
using Questinator.AI.Models;

namespace Questinator.AI.Services
{
    public class AiQuestService
    {
        private readonly HttpClient _http;

        public AiQuestService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AiQuestResult> GenerateQuestAsync(
            string gameEvent,
            int playerLevel)
        {
            // 🔀 Force randomness so quests differ every time
            var randomSeed = Guid.NewGuid().ToString();

            var prompt = $@"
You are a creative game quest designer.

Generate ONE UNIQUE zombie RPG quest.
The quest MUST be different from previous quests.

Rules:
- Short and clear
- Measurable objective
- Objective types may vary:
  (kill, collect, survive, escort, explore)
- Difficulty fits player level {playerLevel}
- Theme: zombies
- Reward (price) must vary (not always the same)
- Do NOT repeat previous quests
- Do NOT explain anything
- Return ONLY valid JSON

RandomSeed: {randomSeed}

JSON format:
{{
  ""questName"": ""..."",
  ""description"": ""..."",
  ""amount"": ""..."",
  ""price"": number
}}

Event: {gameEvent}
";

            var request = new
            {
                model = "llama3",
                prompt = prompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync(
                "http://localhost:11434/api/generate",
                request);

            response.EnsureSuccessStatusCode();

            // 🔥 ALWAYS read raw text first (Ollama is not reliable JSON)
            var rawText = await response.Content.ReadAsStringAsync();

            string modelText;

            // 🔍 Try to parse Ollama wrapper JSON
            try
            {
                using var doc = JsonDocument.Parse(rawText);
                modelText = doc.RootElement
                    .GetProperty("response")
                    .GetString()!;
            }
            catch
            {
                // Fallback: Ollama returned plain text
                modelText = rawText;
            }

            // 🔥 Extract ONLY the JSON object from AI output
            var jsonStart = modelText.IndexOf('{');
            var jsonEnd = modelText.LastIndexOf('}');

            if (jsonStart == -1 || jsonEnd == -1)
            {
                throw new Exception(
                    "No valid JSON found in Ollama response:\n" + modelText
                );
            }

            var json = modelText.Substring(
                jsonStart,
                jsonEnd - jsonStart + 1
            );

            var result = JsonSerializer.Deserialize<AiQuestResult>(json);

            if (result == null)
            {
                throw new Exception(
                    "Failed to deserialize AI quest JSON:\n" + json
                );
            }

            // 🛡️ SAFETY FALLBACKS (AI IS NEVER TRUSTED)
            result.QuestName ??= "Mysterious Zombie Quest";
            result.Description ??= "Complete the objective.";
            result.Amount ??= "1";
            result.Price = NormalizePrice(result.Price);

            return result;
        }

        // 🔒 Clamp reward values to safe range
        private static int NormalizePrice(int price)
        {
            if (price < 10) return 10;
            if (price > 500) return 500;
            return price;
        }
    }
}
