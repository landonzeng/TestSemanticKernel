using Microsoft.SemanticKernel.ChatCompletion;

namespace AIOT.SemanticKernel.Models
{
    public class ChatSession
    {
        public string Account { get; init; } = Guid.NewGuid().ToString();
        public DateTime CreatedTime { get; init; } = DateTime.UtcNow;
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;
        public ChatHistory History { get; init; } = new();
        public int MessageCount => History.Count;
    }
}
