using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.DTOs;
using System.Linq;

namespace LibraryManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowingRequestsController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingRequestsController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        [HttpPost]
        public async Task<ActionResult<BookBorrowingRequest>> CreateBorrowingRequest([FromBody] CreateBorrowingRequestDTO requestDto)
        {
            try
            {
                if (requestDto == null || requestDto.BookIds == null || !requestDto.BookIds.Any())
                {
                    return BadRequest(new { message = "Book IDs are required" });
                }

                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized(new { message = "Invalid user" });
                }

                var request = await _borrowingService.CreateBorrowingRequestAsync(userId, requestDto.BookIds);
                return CreatedAtAction(nameof(GetBorrowingRequestById), new { id = request.Id }, request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookBorrowingRequest>> GetBorrowingRequestById(int id)
        {
            var request = await _borrowingService.GetBorrowingRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<BookBorrowingRequest>>> GetUserBorrowingRequests()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var requests = await _borrowingService.GetUserBorrowingRequestsAsync(userId);
            return Ok(requests);
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<IEnumerable<BookBorrowingRequest>>> GetAllBorrowingRequests()
        {
            var requests = await _borrowingService.GetAllBorrowingRequestsAsync();
            return Ok(requests);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> ApproveBorrowingRequest(int id)
        {
            try
            {
                var approverId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                await _borrowingService.ApproveBorrowingRequestAsync(id, approverId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> RejectBorrowingRequest(int id, [FromBody] string reason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    return BadRequest(new { message = "Reason is required" });
                }

                var approverId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                if (approverId == 0)
                {
                    return Unauthorized(new { message = "Invalid approver" });
                }

                await _borrowingService.RejectBorrowingRequestAsync(id, approverId, reason);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBooks(int id, [FromBody] List<int> bookIds)
        {
            try
            {
                await _borrowingService.ReturnBooksAsync(id, bookIds);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("can-borrow")]
        public async Task<ActionResult<bool>> CanUserBorrowMoreBooks()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var canBorrow = await _borrowingService.CanUserBorrowMoreBooksAsync(userId);
            return Ok(canBorrow);
        }

        [HttpGet("borrowing-count")]
        public async Task<ActionResult<int>> GetUserBorrowingCountThisMonth()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var count = await _borrowingService.GetUserBorrowingCountThisMonthAsync(userId);
            return Ok(count);
        }
    }
}