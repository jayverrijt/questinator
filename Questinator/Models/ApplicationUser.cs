using Microsoft.AspNetCore.Identity;
namespace Questinator.Models;

public class ApplicationUser : IdentityUser
{
// Extra naam die alleen voor weergave is
    public string DisplayName { get; set; } = string.Empty;
}