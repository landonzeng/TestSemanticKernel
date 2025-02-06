using AIOT.SemanticKernel;
using Microsoft.AspNetCore.Mvc;

namespace TestAgentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISemanticKernelService _skService;

        public SessionController(ISemanticKernelService skService)
        {
            _skService = skService;
        }


        [HttpDelete("{account}")]
        public IActionResult RemoveSession(string account)
        {
            _skService.RemoveAccount(account);
            return NoContent();
        }

        [HttpPost("{account}/reset")]
        public IActionResult ResetSession(string account)
        {
            _skService.ResetAccount(account);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetActiveSessions()
        {
            return Ok(_skService.GetActiveAccounts());
        }
    }
}
