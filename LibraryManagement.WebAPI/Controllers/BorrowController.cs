using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBorrows()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                var borrows = await _borrowService.GetBorrowsByUserIdAsync(userId);
                return Ok(borrows);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                var borrow = new Borrow
                {
                    UserId = userId,
                    BookId = request.BookId,
                    BorrowDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(14), // 2 weeks
                    Status = BorrowStatus.Pending
                };

                await _borrowService.CreateBorrowAsync(borrow);
                return CreatedAtAction(nameof(GetBorrowById), new { id = borrow.Id }, borrow);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowById(int id)
        {
            try
            {
                var borrow = await _borrowService.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound(new { message = "Borrow record not found" });

                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                if (borrow.UserId != userId && !User.IsInRole("SuperUser"))
                    return Forbid();

                return Ok(borrow);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBorrow(int id)
        {
            try
            {
                var borrow = await _borrowService.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound(new { message = "Borrow record not found" });

                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                if (borrow.UserId != userId)
                    return Forbid();

                if (borrow.Status != BorrowStatus.Pending)
                    return BadRequest(new { message = "Only pending borrows can be cancelled" });

                await _borrowService.DeleteBorrowAsync(id);
                return Ok(new { message = "Borrow cancelled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            try
            {
                var borrow = await _borrowService.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound(new { message = "Borrow record not found" });

                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                if (borrow.UserId != userId && !User.IsInRole("SuperUser"))
                    return Forbid();

                await _borrowService.ReturnBookAsync(id);
                return Ok(new { message = "Book returned successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CreateBorrowRequest
    {
        public int BookId { get; set; }
    }
} 