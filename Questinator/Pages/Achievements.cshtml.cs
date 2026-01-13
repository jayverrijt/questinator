using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Questinator.Data;
using Questinator.Models;

namespace Questinator.Pages
{
    [Authorize]
    public class AchievementsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AchievementsModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<AchievementViewModel> Achievements { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            Achievements = await _context.Achievements
                .Where(a => a.UserId == user.Id)
                .Select(a => new AchievementViewModel
                {
                    Text = a.AchievementText,
                    FromQuest = a.FromQuest
                })
                .ToListAsync();
        }
    }
    public class AchievementViewModel
    {
        public string Text { get; set; }
        public int? FromQuest { get; set; }
    }

}