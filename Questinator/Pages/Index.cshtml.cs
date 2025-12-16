using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Questinator.Pages
{
    [Authorize] // 🔒 Alleen ingelogde users
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Extra check, voor het geval Authorize niet werkt
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Info"); // stuur niet-ingelogde users naar /Info
            }

            return Page();
        }
    }
}