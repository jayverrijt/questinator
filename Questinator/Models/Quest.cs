using System.ComponentModel.DataAnnotations;

namespace Questinator.Models
{
    public class Quest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QuestName { get; set; }

        public string Description { get; set; }

        public int Status { get; set; } // 1 = ongoing, 2 = completed

        public string UserId { get; set; }

        public string? Amount { get; set; }

        public int Price { get; set; }

        public bool IsSkipped { get; set; } = false; // Nieuwe property
    }
}