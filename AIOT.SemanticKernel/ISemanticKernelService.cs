using AIOT.SemanticKernel.Models;

namespace AIOT.SemanticKernel
{
    public interface ISemanticKernelService
    {
        IAsyncEnumerable<string> GetStreamingResponseAsync(string message, string account);
        Task<string> GetCompleteResponseAsync(string message, string account);
        void ResetAccount(string account);
        void RemoveAccount(string account);
        IEnumerable<ChatSession> GetActiveAccounts();
        ChatSession? GetAccountSession(string account);
    }
}
