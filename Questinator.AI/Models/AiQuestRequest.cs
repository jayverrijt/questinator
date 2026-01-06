namespace Questinator.AI.Models
{
    public class AiQuestRequest
    {
        public string UserId { get; set; }
        public string Event { get; set; } // NPC / QUEST_COMPLETED
        public int PlayerLevel { get; set; }
    }
}