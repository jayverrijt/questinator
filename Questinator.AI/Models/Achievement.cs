using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questinator.AI.Models
{
    public class Achievement
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? FromQuest { get; set; }

        public string AchievementText { get; set; }
    }


}