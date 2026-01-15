using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Http.Json;
using System;

namespace Questinator.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult OnGet()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Info");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostStartGame()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // API call naar jouw Session Token Service
            var response = await _httpClient.PostAsJsonAsync(
                "http://192.168.132.124:5076/api/session/create",
                new { UserId = userId }
            );

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to start session");

            var result = await response.Content.ReadFromJsonAsync<SessionTokenResponse>();

            if (result == null)
                return StatusCode(500, "No token received from Session API");

            // Unity game URL
            var gameUrl = $"questinator://play?session_token={result.SessionToken}";

            return Redirect(gameUrl);
        }

        public class SessionTokenResponse
        {
            public string SessionToken { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    }
}