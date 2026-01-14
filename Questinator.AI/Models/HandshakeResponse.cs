namespace Questinator.AI.Models;

public class HandshakeResponse
{
    public string UserId { get; set; }
    public List<QuestDto> ActiveQuests { get; set; }
    public DateTime ExpiresAt { get; set; }
}
