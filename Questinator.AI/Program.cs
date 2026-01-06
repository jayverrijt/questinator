using Microsoft.EntityFrameworkCore;
using Questinator.AI.Data;
using Questinator.AI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ✅ EF Core + SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ✅ Ollama AI service
builder.Services.AddHttpClient<AiQuestService>();

var app = builder.Build();

app.MapControllers();

app.Run();