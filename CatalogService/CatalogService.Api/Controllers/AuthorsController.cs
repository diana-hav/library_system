using CatalogService.Bll.Dtos;
using CatalogService.Bll.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AuthorService _service;

    public AuthorsController(AuthorService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuthorDto>), 200)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
    {
        var authors = await _service.GetAllAsync();
        return Ok(authors);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AuthorDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<AuthorDto>> Create([FromBody] AuthorDto dto)
    {
        var createdDto = await _service.CreateAsync(dto);

        if (createdDto == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetAll), new { id = createdDto.Id }, createdDto);
    }
}
