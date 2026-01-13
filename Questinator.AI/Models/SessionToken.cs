using System;

namespace Questinator.AI.Models
{
    public class SessionToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string TokenHash { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}