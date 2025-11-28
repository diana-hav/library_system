using CatalogService.Bll.Dtos;
using CatalogService.Bll.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly GenreService _service;

    public GenresController(GenreService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GenreDto>), 200)]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll()
    {
        var genres = await _service.GetAllAsync();
        return Ok(genres);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GenreDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<GenreDto>> Create([FromBody] GenreDto dto)
    {
        var createdDto = await _service.CreateAsync(dto);

        if (createdDto == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetAll), new { id = createdDto.Id }, createdDto);
    }
}
