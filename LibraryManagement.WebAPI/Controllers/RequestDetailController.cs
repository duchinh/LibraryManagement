using LibraryManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Services;
using LibraryManagement.Core.Interfaces;


namespace LibraryManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestDetailController : ControllerBase
    {
        private readonly IBorrowRequestDetailService _service;

        public RequestDetailController(IBorrowRequestDetailService service)
        {
            _service = service;
        }

        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetDetailsByRequestId(Guid requestId)
        {
            var details = await _service.GetDetailsByRequestIdAsync(requestId);
            if (details == null || details.Count == 0)
            {

                return NotFound();
            }

            return Ok(details);
        }
    }
}
