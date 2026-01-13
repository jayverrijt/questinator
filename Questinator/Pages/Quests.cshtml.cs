using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Questinator.Data;
using Questinator.Models;

namespace Questinator.Pages
{
    [Authorize]
    public class QuestsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Quest> CompletedQuests { get; set; } = new();
        public List<Quest> OngoingQuests { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return;

            var quests = await _context.Quests
                .Where(q => q.UserId == user.Id)
                .ToListAsync();

            CompletedQuests = quests.Where(q => q.Status == 2).ToList();
            OngoingQuests = quests.Where(q => q.Status == 1 || q.IsSkipped).ToList();
        }

        // 🔥 SKIP QUEST + COINS + TRANSACTION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSkipQuest([FromBody] SkipQuestRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var quest = await _context.Quests
                .FirstOrDefaultAsync(q => q.Id == request.QuestId && q.UserId == user.Id);

            if (quest == null || quest.IsSkipped)
                return BadRequest();

            const int skipCost = 75;

            // ❌ Niet genoeg coins
            if (user.Coins < skipCost)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = "NOT_ENOUGH_COINS"
                })
                {
                    StatusCode = 400
                };
            }

            // ✅ Quest skippen
            quest.IsSkipped = true;
            quest.Status = 1;

            // ✅ Coins aftrekken
            user.Coins -= skipCost;

            // ✅ Coin transaction loggen (BESTAAND MODEL)
            var transaction = new CoinTransaction
            {
                UserId = user.Id,
                QuestId = quest.Id,
                Amount = -skipCost,
                Type = "SKIP_QUEST",
                Description = $"Skip quest: {quest.QuestName}",
                CreatedAt = DateTime.UtcNow
            };

            _context.Quests.Update(quest);
            _context.Users.Update(user);
            _context.CoinTransactions.Add(transaction);

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                newBalance = user.Coins
            });
        }

        public class SkipQuestRequest
        {
            public int QuestId { get; set; }
        }
    }
}
