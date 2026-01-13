using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Questinator.Data;
using Questinator.Models;

namespace Questinator.Pages
{
    [Authorize]
    public class CoinsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoinsModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CoinTransaction> Transactions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            Transactions = await _context.CoinTransactions
                .Where(t => t.UserId == user.Id)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}