using Microsoft.AspNetCore.Mvc;
using BorrowingService.Bll.Services;

namespace BorrowingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingsController : ControllerBase
{
    private readonly BorrowingAppService _service;

    public BorrowingsController(BorrowingAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpPost("issue")]
    public async Task<IActionResult> IssueBook([FromBody] IssueBookRequest request)
    {
        try
        {
            var id = await _service.IssueBookAsync(request.ReaderId, request.BookId);
            return CreatedAtAction(nameof(GetAll), new { id }, new { message = "Book issued successfully", id });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    public class IssueBookRequest
    {
        public int ReaderId { get; set; }
        public int BookId { get; set; }
    }

    [HttpPut("return/{id}")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        try
        {
            await _service.ReturnBookAsync(id);
            return Ok(new { message = "Book returned successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}