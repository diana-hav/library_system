using Microsoft.AspNetCore.Mvc;
using BorrowingService.Bll.Services;
using BorrowingService.Bll.Dtos;

namespace BorrowingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadersController : ControllerBase
{
    private readonly ReaderService _service;
    public ReadersController(ReaderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var readers = await _service.GetAllAsync();
        return Ok(readers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reader = await _service.GetByIdAsync(id);
        if (reader == null) return NotFound();
        return Ok(reader);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReaderDto dto)
    {
        var id = await _service.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }
}
