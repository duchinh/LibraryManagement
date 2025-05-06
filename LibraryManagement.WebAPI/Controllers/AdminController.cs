using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "SuperUser")]
    public class AdminController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        private readonly IUserService _userService;
        private readonly IReportGenerator _reportGenerator;

        public AdminController(
            IBorrowService borrowService,
            IUserService userService,
            IReportGenerator reportGenerator)
        {
            _borrowService = borrowService;
            _userService = userService;
            _reportGenerator = reportGenerator;
        }

        [HttpGet("borrow-requests")]
        public async Task<IActionResult> GetBorrowRequests([FromQuery] string? status)
        {
            try
            {
                var borrows = await _borrowService.GetAllBorrowsAsync();
                if (!string.IsNullOrEmpty(status))
                {
                    borrows = borrows.Where(b => b.Status.ToString() == status);
                }
                return Ok(borrows);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("borrow-requests/{id}/approve")]
        public async Task<IActionResult> ApproveBorrowRequest(int id)
        {
            try
            {
                var borrow = await _borrowService.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound(new { message = "Borrow request not found" });

                if (borrow.Status != BorrowStatus.Pending)
                    return BadRequest(new { message = "Only pending requests can be approved" });

                borrow.Status = BorrowStatus.Approved;
                await _borrowService.UpdateBorrowAsync(borrow);
                return Ok(new { message = "Borrow request approved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("borrow-requests/{id}/reject")]
        public async Task<IActionResult> RejectBorrowRequest(int id, [FromBody] RejectRequest request)
        {
            try
            {
                var borrow = await _borrowService.GetBorrowByIdAsync(id);
                if (borrow == null)
                    return NotFound(new { message = "Borrow request not found" });

                if (borrow.Status != BorrowStatus.Pending)
                    return BadRequest(new { message = "Only pending requests can be rejected" });

                borrow.Status = BorrowStatus.Rejected;
                borrow.Notes = request.Reason;
                await _borrowService.UpdateBorrowAsync(borrow);
                return Ok(new { message = "Borrow request rejected successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> ChangeUserRole(int id, [FromBody] ChangeRoleRequest request)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                user.Role = request.Role;
                await _userService.UpdateUserAsync(user);
                return Ok(new { message = "User role updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GenerateReport([FromQuery] string type, [FromQuery] string? startDate, [FromQuery] string? endDate)
        {
            try
            {
                var report = await _reportGenerator.GenerateBorrowReportAsync(type, startDate, endDate);
                return File(report, "application/octet-stream", $"report.{type}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = new
                {
                    TotalBooks = await _borrowService.GetAllBorrowsAsync(),
                    TotalUsers = await _userService.GetAllUsersAsync(),
                    PendingRequests = (await _borrowService.GetAllBorrowsAsync()).Count(b => b.Status == BorrowStatus.Pending),
                    OverdueBooks = (await _borrowService.GetAllBorrowsAsync()).Count(b => b.Status == BorrowStatus.Approved && b.DueDate < DateTime.UtcNow)
                };
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class RejectRequest
    {
        public string Reason { get; set; } = string.Empty;
    }

    public class ChangeRoleRequest
    {
        public UserRole Role { get; set; }
    }
}