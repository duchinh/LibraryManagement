using LibraryManagement.Core.DTOs;
using LibraryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Services;
using LibraryManagement.Core.Interfaces.Services;

namespace LibraryManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();

            return Ok(books);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)

                return NotFound();

            return Ok(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO dto)
        {
            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var book = await _bookService.CreateBookAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedBook = await _bookService.UpdateBookAsync(id, dto);
            if (updatedBook == null)
                return NotFound();

            return Ok(updatedBook);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result)

                return NotFound();

            return NoContent();
        }
    }
}

