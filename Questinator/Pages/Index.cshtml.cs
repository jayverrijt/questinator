using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Questinator.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Info");
            }

            return Page();
        }

        // 🎮 START GAME
        public IActionResult OnPostStartGame()
        {
            // ✅ UserId ophalen uit Identity
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // 🔮 HIER KOMT STRAKS DE API VAN JE COLLEGA
            //
            // Voorbeeld (API call):
            // await _gameApi.StartGame(userId);

            // Voor nu: redirect met userId (mock)
            var gameUrl = $"https://example-game-url.com/start?userId={userId}";

            return Redirect(gameUrl);
        }
    }
}