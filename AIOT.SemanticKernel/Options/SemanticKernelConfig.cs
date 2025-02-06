namespace AIOT.SemanticKernel.Options
{
    public class SemanticKernelConfig
    {
        public required string ApiKey { get; set; } = "ollama";
        public required string ModelId { get; set; } = "qwen2:7b";
        public string? Endpoint { get; set; } = "http://localhost:11434/v1/";
        public int MaxTokens { get; set; } = 4096;
        public double Temperature { get; set; } = 0.3;
    }
}
