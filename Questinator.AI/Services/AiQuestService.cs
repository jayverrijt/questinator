using System.Text.Json;
using System.Text.Json.Serialization;
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

        public async Task<AiQuestResult> GenerateQuestAsync(string gameEvent, int playerLevel)
        {
            var prompt = $@"
You are an AI Quest Generator for a zombie RPG.

Generate one NEW quest every time.
Quest must be either KILL or COLLECT.
Do not repeat previous output.

JSON ONLY in this format:

{{
  ""questName"": ""string"",
  ""description"": ""string"",
  ""type"": ""KILL | COLLECT"",
  ""amount"": number,
  ""price"": number
}}
";
            var request = new
            {
                model = "llama3.1-instruct",
                prompt = prompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", request);
            var raw = await response.Content.ReadAsStringAsync();

            string json = ExtractJson(raw);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var ai = JsonSerializer.Deserialize<AiQuestResult>(json, options);

            // Fallbacks
            if (ai == null)
                ai = new AiQuestResult();

            if (ai.Amount <= 0)
                ai.Amount = Random.Shared.Next(3, 30);

            if (ai.Price <= 0)
                ai.Price = Random.Shared.Next(10, 100);

            if (ai.Type == 0)
                ai.Type = Random.Shared.Next(0, 2) == 0 ? QuestType.Kill : QuestType.Collect;

            if (string.IsNullOrWhiteSpace(ai.QuestName))
                ai.QuestName = ai.Type == QuestType.Kill ? GenerateRandomKillName() : GenerateRandomCollectName();

            if (string.IsNullOrWhiteSpace(ai.Description))
                ai.Description = ai.Type == QuestType.Kill ? GenerateRandomKillDesc() : GenerateRandomCollectDesc();

            return ai;
        }


        private string ExtractJson(string text)
        {
            int start = text.IndexOf("{");
            int end = text.LastIndexOf("}");

            if (start == -1 || end == -1)
                throw new Exception("No valid JSON found in Ollama response:\n" + text);

            return text.Substring(start, end - start + 1);
        }

        private string GenerateRandomName()
        {
            string[] names = {
                "Zombie Purge",
                "Graveyard Sweep",
                "Dead Rising Alert",
                "Night Walker Hunt",
                "Rotting Horde Breakout",
                "Ghoul Interference"
            };

            return names[Random.Shared.Next(names.Length)];
        }

        private string GenerateRandomDescription()
        {
            string[] desc = {
                "Eliminate a nearby zombie pack before they regroup.",
                "Secure the graveyard and neutralize undead threats.",
                "Stop a rising outbreak spreading through the streets.",
                "Hunt a scouting group of night walkers.",
                "Disrupt a zombie horde preparing to migrate.",
                "Prevent ghouls from overtaking a nearby camp."
            };

            return desc[Random.Shared.Next(desc.Length)];
        }
        private string GenerateRandomKillName()
        {
            string[] names = {
                "Zombie Purge",
                "Bone Ripper Hunt",
                "Night Walker Extermination",
                "Flesh Eater Sweep"
            };
            return names[Random.Shared.Next(names.Length)];
        }

        private string GenerateRandomCollectName()
        {
            string[] names = {
                "Collect Blood Samples",
                "Retrieve Toxic Vials",
                "Gather Virus Data",
                "Secure Stolen Supplies"
            };
            return names[Random.Shared.Next(names.Length)];
        }

        private string GenerateRandomKillDesc()
        {
            string[] desc = {
                "Eliminate a roaming pack of undead.",
                "Stop a scout patrol of infected grotesques.",
                "Purge walkers before they reach camp."
            };
            return desc[Random.Shared.Next(desc.Length)];
        }

        private string GenerateRandomCollectDesc()
        {
            string[] desc = {
                "Collect hazardous samples for research.",
                "Retrieve stolen medical cargo.",
                "Gather supplies to support survivors."
            };
            return desc[Random.Shared.Next(desc.Length)];
        }

    }
}
