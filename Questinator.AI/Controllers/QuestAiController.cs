using Microsoft.AspNetCore.Mvc;
using Questinator.AI.Services;
using Questinator.AI.Data;
using Questinator.AI.Models;
using Questinator.AI.Security;

namespace Questinator.AI.Controllers
{
    [ApiController]
    [Route("api/ai/quests")]
    [ApiKey] // 🔐 HIER
    public class QuestAiController : ControllerBase
    {
        private readonly AiQuestService _ai;
        private readonly ApplicationDbContext _db;

        public QuestAiController(
            AiQuestService ai,
            ApplicationDbContext db)
        {
            _ai = ai;
            _db = db;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQuest(
            [FromBody] AiQuestRequest request)
        {
            var aiQuest = await _ai.GenerateQuestAsync(
                request.Event,
                request.PlayerLevel);

            var quest = new Quest
            {
                UserId = request.UserId,
                QuestName = aiQuest.QuestName ?? "Unknown Quest",
                Description = aiQuest.Description ?? "Complete the objective.",
                Amount = aiQuest.Amount ?? "1",
                Price = aiQuest.Price > 0 ? aiQuest.Price : 50,
                Status = 1
            };


            _db.Quests.Add(quest);
            await _db.SaveChangesAsync();

            return Ok(quest);
        }
    }
}