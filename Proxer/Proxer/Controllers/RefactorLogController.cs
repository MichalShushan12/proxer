using BusinessLogic.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Proxer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefactorLogController : ControllerBase
    {
        private readonly IRefactorLogService _refactorLogService;

        public RefactorLogController(IRefactorLogService refactorLogService)
        {
            _refactorLogService = refactorLogService;
        }

        [EnableRateLimiting("api")]
        [HttpPost("process")]
        public async Task<IActionResult> ProcessCode([FromBody] RefactorRequestDTO request)
        {

            // כרגע אנחנו שמים חומרה קשיחה (Hardcoded) של משתמש מספר 1 כדי לבדוק שהכל זורם.
            // בהמשך, ה-UserId יישלף בצורה מאובטחת מתוך ה-ApiKey או ה-Token שהתוסף ישלח!
            int temporaryUserId = 1;

            if (request == null || string.IsNullOrEmpty(request.OriginalCode))
            {
                return BadRequest("Invalid request or empty code.");
            }

            var result = await _refactorLogService.ExecuteRefactorAsync(request, temporaryUserId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }
    }
}
