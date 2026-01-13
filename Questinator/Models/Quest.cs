using Questinator.AI.Models;

public class Quest
{
    public int Id { get; set; }
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public string UserId { get; set; }
    public QuestType Type { get; set; }
    public int Amount { get; set; }
    public int Price { get; set; }
    public bool IsSkipped { get; set; } = false;
    public int Progress { get; set; } = 0;
}