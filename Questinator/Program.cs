using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Questinator.Data;
using Questinator.Models;

var builder = WebApplication.CreateBuilder(args);

//
// 🔹 Database (MSSQL)
//
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//
// 🔹 Identity met ApplicationUser
//
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        // Password instellingen
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

//
// 🔹 Cookie instellingen
//
builder.Services.ConfigureApplicationCookie(options =>
{
    // Niet-ingelogde users gaan automatisch naar /Info
    options.LoginPath = "/Info";
    options.AccessDeniedPath = "/Info";
});

//
// 🔹 Razor Pages + Authorisatie
//
builder.Services.AddRazorPages(options =>
{
    // 🔒 Alle pagina’s vereisen login
    options.Conventions.AuthorizeFolder("/");

    // 🔓 Publieke pagina’s
    options.Conventions.AllowAnonymousToPage("/Info");

    // 🔓 Identity (Login/Register/Logout/Manage)
    options.Conventions.AllowAnonymousToAreaFolder("Identity", "/");
});

var app = builder.Build();

//
// 🔹 Middleware pipeline
//
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();