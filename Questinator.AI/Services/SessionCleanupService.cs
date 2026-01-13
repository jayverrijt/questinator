using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class SessionCleanupService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

    public SessionCleanupService(IConfiguration config)
    {
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                await con.OpenAsync(stoppingToken);

                var cmd = new SqlCommand(
                    "DELETE FROM SessionTokens WHERE ExpiresAt <= SYSUTCDATETIME()", con);

                await cmd.ExecuteNonQueryAsync(stoppingToken);
            }
            catch (SqlException ex)
            {
                // tabel bestaat (nog) niet → negeren
                if (ex.Message.Contains("Invalid object name 'SessionTokens'"))
                {
                    // stille fallback → BREAK of CONTINUE mogelijk
                }
                else
                {
                    throw; // andere DB fouten moeten blijven bestaan
                }
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}