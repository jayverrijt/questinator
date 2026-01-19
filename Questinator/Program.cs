using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Questinator.Data;
using Questinator.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("Questinator", client =>
{
    client.DefaultRequestHeaders.Add("X-API-KEY", "WURST_WURST_WURST_WURST_WURST");
});


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


builder.Services.ConfigureApplicationCookie(options =>
{
    // Niet-ingelogde users gaan automatisch naar /Info
    options.LoginPath = "/Info";
    options.AccessDeniedPath = "/Info";
});


builder.Services.AddRazorPages(options => {});

var app = builder.Build();


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