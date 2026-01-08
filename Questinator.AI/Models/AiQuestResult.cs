namespace Questinator.AI.Models
{
    public class AiQuestResult
    {
        public string QuestName { get; set; }
        public string Description { get; set; }
        public QuestType Type { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
    }

}