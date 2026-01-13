using Microsoft.AspNetCore.Identity;

namespace Questinator.AI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Coins { get; set; }
    }
}