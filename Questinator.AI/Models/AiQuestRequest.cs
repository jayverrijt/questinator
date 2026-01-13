namespace Questinator.AI.Models
{
    public class AiQuestRequest
    {
        public string UserId { get; set; }
        public string Event { get; set; }
        public int PlayerLevel { get; set; }
    }
}