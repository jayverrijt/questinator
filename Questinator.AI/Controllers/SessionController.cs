using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Questinator.AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SessionController(IConfiguration config)
        {
            _config = config;
        }

        public class SessionTokenRequest
        {
            public string UserId { get; set; }
        }

        public class SessionTokenResponse
        {
            public string SessionToken { get; set; }
            public DateTime ExpiresAt { get; set; }
        }

        public class ValidateRequest
        {
            public string Token { get; set; }
        }

        [HttpPost("create")]
        public async Task<ActionResult<SessionTokenResponse>> Create([FromBody] SessionTokenRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.UserId))
                return BadRequest("UserId is required");

            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            var hashedToken = Convert.ToBase64String(hashedBytes);
            var expiresAt = DateTime.UtcNow.AddHours(1);

            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await con.OpenAsync();
            var cmd = new SqlCommand(@"
                INSERT INTO SessionTokens (UserId, TokenHash, ExpiresAt)
                VALUES (@u, @h, @e)", con);

            cmd.Parameters.AddWithValue("@u", req.UserId);
            cmd.Parameters.AddWithValue("@h", hashedToken);
            cmd.Parameters.AddWithValue("@e", expiresAt);
            await cmd.ExecuteNonQueryAsync();

            return new SessionTokenResponse
            {
                SessionToken = rawToken,
                ExpiresAt = expiresAt
            };
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] ValidateRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Token))
                return BadRequest("Missing token");

            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(req.Token));
            var hashedToken = Convert.ToBase64String(hashedBytes);

            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await con.OpenAsync();
            var cmd = new SqlCommand(@"
                SELECT 1 FROM SessionTokens
                WHERE TokenHash = @h AND ExpiresAt > SYSUTCDATETIME()", con);

            cmd.Parameters.AddWithValue("@h", hashedToken);

            var result = await cmd.ExecuteScalarAsync();
            if (result == null)
                return Unauthorized();

            return Ok();
        }
    }
}
