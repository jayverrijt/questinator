using Microsoft.EntityFrameworkCore;
using Questinator.AI.Data;
using Questinator.AI.Models;
using Questinator.AI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHttpClient();


// ✅ Ollama AI service
builder.Services.AddHttpClient<AiQuestService>();

var app = builder.Build();

app.MapControllers();

app.Run();