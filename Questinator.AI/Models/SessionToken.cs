namespace Questinator.AI.Models
{

    public class SessionTokenRequest
    {
        public string UserId { get; set; }
    }

    public class SessionTokenResponse
    {
        public string SessionToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

}