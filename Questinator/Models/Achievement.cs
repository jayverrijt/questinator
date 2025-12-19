using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questinator.Models
{
    public class Achievement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // foreign key naar ApplicationUser

        public int? FromQuest { get; set; } // kan NULL zijn, refereert naar quest ID

        [Required]
        public string AchievementText { get; set; }
    }
}