using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Questinator.Models;

namespace Questinator.Pages
{
    //[Authorize]
    public class AchievementsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AchievementsModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public List<AchievementViewModel> Achievements { get; set; } = new();

        public void OnGet()
        {
            // Hardcoded achievement met questnaam in plaats van points
            Achievements = new List<AchievementViewModel>
            {
                new AchievementViewModel
                {
                    Name = "Haha",
                    Description = "Complete this secret achievement to unlock the mysteries",
                    QuestName = "Funny Quest"
                }
            };
        }
    }

    public class AchievementViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Nieuwe property voor de naam van de quest
        public string QuestName { get; set; }
    }
}