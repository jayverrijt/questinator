using System;

namespace Questinator.Models
{
    public class CoinTransaction
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? QuestId { get; set; }

        public int Amount { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}