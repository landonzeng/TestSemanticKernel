namespace AIOT.SemanticKernel.Options
{
    public class SessionOptions
    {
        public TimeSpan SessionTimeout { get; set; } = TimeSpan.FromMinutes(30);
        public int MaxSessions { get; set; } = 100;
        public TimeSpan CleanupInterval { get; set; } = TimeSpan.FromMinutes(5);
    }
}
