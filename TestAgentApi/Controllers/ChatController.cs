using AIOT.SemanticKernel;
using AIOT.SemanticKernel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TestAgentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ISemanticKernelService _skService;

        public ChatController(ISemanticKernelService skService)
        {
            _skService = skService;
        }

        [HttpPost("stream/{account}")]
        public async Task StreamChat(string account, [FromBody] QuestionDTO input)
        {
            Response.ContentType = "text/event-stream";         

            try
            {
                await foreach (var chunk in _skService.GetStreamingResponseAsync(input.Question, account))
                {
                    await Response.WriteAsync($"data: {MessageModel<string>.Ok(chunk.ToString())}\n");
                    await Response.Body.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                var error = JsonSerializer.Serialize(MessageModel<string>.Fail($"Éú³ÉÊ§°Ü:{ex.Message}"));
                await Response.WriteAsync($"data: {ex.Message}\n\n"); 
                await Response.Body.FlushAsync();
            }
        }

        [HttpPost("complete/{account}")]
        public async Task<IActionResult> CompleteChat(string account, [FromBody] QuestionDTO input)
        {
            try
            {
                var response = await _skService.GetCompleteResponseAsync(input.Question, account);
                return Ok(new { Response = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
    public class QuestionDTO
    {
        public string Question { get; set; }
    }
}
