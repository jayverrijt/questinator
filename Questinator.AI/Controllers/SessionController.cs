using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Questinator.AI.Models;

namespace Questinator.AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        [HttpPost("create")]
        public ActionResult<SessionTokenResponse> Create([FromBody] SessionTokenRequest req)
        {
            if (string.IsNullOrEmpty(req.UserId))
                return BadRequest("UserId is required");

            // generate random session token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            var response = new SessionTokenResponse
            {
                SessionToken = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            // TODO: store token in DB if needed

            return Ok(response);
        }
    }
}