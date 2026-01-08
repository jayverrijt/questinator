using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Questinator.AI.Data;
using Questinator.AI.Models;
using System.Net.Http.Json;

namespace Questinator.AI.Controllers
{
    [ApiController]
    [Route("api/quests")]
    public class QuestsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuestsApiController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteQuest([FromBody] CompleteQuestRequest request)
        {
            var quest = await _context.Quests
                .FirstOrDefaultAsync(q => q.Id == request.QuestId);

            if (quest == null)
                return NotFound("Quest not found");

            if (quest.Status == 2)
                return BadRequest("Already completed");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == quest.UserId);

            if (user == null)
                return NotFound("User not found");

            // 1. Mark as completed
            quest.Status = 2;

            // 2. Give coins
            user.Coins += quest.Price;

            // 3. Log transaction
            _context.CoinTransactions.Add(new CoinTransaction
            {
                UserId = user.Id,
                QuestId = quest.Id,
                Amount = quest.Price,
                Type = "QUEST_REWARD",
                Description = $"Completed quest '{quest.QuestName}'"
            });

            await _context.SaveChangesAsync();

            // 4. Generate new quest automatically
            await GenerateNewQuestAsync(user.Id);

            return Ok(new { success = true });
        }

        private async Task GenerateNewQuestAsync(string userId)
        {
            var client = _httpClientFactory.CreateClient();
            var payload = new { userId, @event = "QUEST_COMPLETED", playerLevel = 1 };

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5076/api/ai/quests/generate")
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Add("X-API-KEY", "WURST_WURST_WURST_WURST_WURST");

            await client.SendAsync(request);
        }
    }

    public class CompleteQuestRequest
    {
        public int QuestId { get; set; }
    }
}
