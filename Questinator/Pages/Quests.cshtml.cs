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

        [HttpPost]
        public async Task<IActionResult> OnPostSkipQuest([FromBody] SkipQuestRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var quest = await _context.Quests
                .Where(q => q.Id == request.QuestId && q.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (quest == null || quest.IsSkipped) return BadRequest();

            // Mark as skipped and charge coins
            quest.IsSkipped = true;
            quest.Status = 1; // stays as ongoing
            user.Coins -= 75;

            _context.Update(quest);
            _context.Update(user);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public class SkipQuestRequest
        {
            public int QuestId { get; set; }
        }
    }
}
