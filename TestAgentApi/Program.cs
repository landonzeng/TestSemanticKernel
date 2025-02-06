using AIOT.SemanticKernel;
using AIOT.SemanticKernel.Options;
using SessionOptions = AIOT.SemanticKernel.Options.SessionOptions;

namespace TestAgentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new SemanticKernelConfig
            {
                ApiKey = builder.Configuration["Ollama:ApiKey"] ?? "ollama",
                ModelId = builder.Configuration["Ollama:ModelId"] ?? "qwen2.5:14b",
                Endpoint = builder.Configuration["Ollama:Endpoint"] ?? "http://localhost:11434/v1/",
                MaxTokens = int.TryParse(builder.Configuration["Ollama:MaxTokens"], out var mt) ? mt : 4096,
                Temperature = double.TryParse(builder.Configuration["Ollama:Temperature"], out var temp) ? temp : 0.3
            };

            var sessionOptions = new SessionOptions
            {
                MaxSessions = int.TryParse(builder.Configuration["Session:MaxSessions"], out var ms) ? ms : 100,
                SessionTimeout = TimeSpan.TryParse(builder.Configuration["Session:Timeout"], out var st) ? st : TimeSpan.FromMinutes(30),
                CleanupInterval = TimeSpan.TryParse(builder.Configuration["Session:Cleanup"], out var ci) ? ci : TimeSpan.FromMinutes(5)
            };

            builder.Services.AddSingleton<ISemanticKernelService>(_ => new SemanticKernelService(config, sessionOptions));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy
                    (name: "AllowAllOrigins",
                        builde =>
                        {
                            builde.WithOrigins("*", "*", "*")
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        }
                    );
            });

            var app = builder.Build();

            app.UseStaticFiles();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");
            app.MapControllers();

            app.Run();
        }
    }
}
