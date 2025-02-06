using AIOT.SemanticKernel.Models;
using AIOT.SemanticKernel.Options;
using AIOT.SemanticKernel.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Collections.Concurrent;
using System.Text;

namespace AIOT.SemanticKernel
{
    public class SemanticKernelService : ISemanticKernelService, IDisposable
    {
        private readonly Kernel _kernel;
        private readonly ConcurrentDictionary<string, ChatSession> _sessions;
        private readonly Timer _cleanupTimer;
        private readonly SemanticKernelConfig _config;
        private readonly SessionOptions _options;

        #pragma warning disable SKEXP0010
        public SemanticKernelService(SemanticKernelConfig config, SessionOptions options)
        {
            _config = config;
            _options = options;
     
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion
            (
                modelId: config.ModelId,
                apiKey: config.ApiKey,
                endpoint: new Uri(config.Endpoint ?? "http://localhost:11434/v1/")
            );

            builder.Plugins.AddFromType<TimesPlugin>();
            builder.Plugins.AddFromType<WeatherPlugin>();
            builder.Plugins.AddFromType<IPAddressPlugin>();

            _kernel = builder.Build();
            _sessions = new ConcurrentDictionary<string, ChatSession>();

            _cleanupTimer = new Timer(_ => CleanupExpiredSessions(),
                null,
                _options.CleanupInterval,
                _options.CleanupInterval);
        }

        public IAsyncEnumerable<string> GetStreamingResponseAsync(string message, string account)
        {
            var session = _sessions.GetOrAdd(account, _ => new ChatSession
            {
                Account = account,
                CreatedTime = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            }); 

            session.LastActivity = DateTime.UtcNow;

            session.History.AddUserMessage(message);
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            var settings = new OpenAIPromptExecutionSettings
            {
                MaxTokens = _config.MaxTokens,
                Temperature = _config.Temperature,
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),    //如果注释掉这行代码，那么流式输出就正常了,但是插件功能也消失了
            };

            var fullResponse = new StringBuilder();

            var stream = chatService.GetStreamingChatMessageContentsAsync
                (
                    session.History,
                    settings,
                    _kernel
                );

            return ProcessStream(stream, session, fullResponse);
        }

        private async IAsyncEnumerable<string> ProcessStream(
            IAsyncEnumerable<StreamingChatMessageContent> stream,
            ChatSession session,
            StringBuilder fullResponse)
        {
            await foreach (var chunk in stream)
            {
                session.LastActivity = DateTime.UtcNow;

                var content = ProcessChunk(chunk);
                if (string.IsNullOrEmpty(content)) continue;

                fullResponse.Append(content);
                yield return content;
            }

            session.History.AddAssistantMessage(fullResponse.ToString());
        }

        private string ProcessChunk(StreamingChatMessageContent chunk)
        {
            if (chunk.Content?.StartsWith("{") == true)
            {
                try
                {
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(chunk.Content);
                    return json.message?.content?.ToString() ?? string.Empty;
                }
                catch
                {
                    return chunk.Content;
                }
            }
            return chunk.Content ?? string.Empty;
        }

        public async Task<string> GetCompleteResponseAsync(string message, string account)
        {
            var response = new StringBuilder();
            await foreach (var chunk in GetStreamingResponseAsync(message, account))
            {
                response.Append(chunk);
            }
            return response.ToString();
        }

        public void ResetAccount(string account)
        {
            if (_sessions.TryGetValue(account, out var session))
            {
                session.History.Clear();
            }
        }

        public void RemoveAccount(string account)
        {
            _sessions.TryRemove(account, out _);
        }

        public IEnumerable<ChatSession> GetActiveAccounts()
        {
            return _sessions.Values;
        }

        public ChatSession? GetAccountSession(string account)
        {
            return _sessions.TryGetValue(account, out var session) ? session : null;
        }

        private void CleanupExpiredSessions()
        {
            var cutoff = DateTime.UtcNow - _options.SessionTimeout;
            var expiredSessions = _sessions
                .Where(kv => kv.Value.LastActivity < cutoff)
                .Select(kv => kv.Key)
                .ToList();

            foreach (var account in expiredSessions)
            {
                _sessions.TryRemove(account, out _);
            }
        }

        public void Dispose()
        {
            _cleanupTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
