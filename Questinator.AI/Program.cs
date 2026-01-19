using Microsoft.EntityFrameworkCore;
using Questinator.AI.Data;
using Questinator.AI.Models;
using Questinator.AI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<SessionCleanupService>();
builder.Services.AddControllers();

builder.Services.AddHttpClient<AiQuestService>(client =>
{
    var config = builder.Configuration;
    var url = config["Ollama:Url"] ?? "http://localhost:11434";
    client.BaseAddress = new Uri(url);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHttpClient();


builder.Services.AddHttpClient<AiQuestService>();

var app = builder.Build();

app.MapControllers();

app.Run();