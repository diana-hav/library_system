using CatalogService.Bll.Dtos;
using CatalogService.Bll.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _service;

        public BooksController(BookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _service.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPaged([FromQuery] QueryParametersDto parameters)
        {
            var result = await _service.GetPagedAsync(parameters);

            return Ok(new
            {
                items = result.Items,
                page = result.Page,
                pageSize = result.PageSize,
                totalCount = result.TotalCount,
                totalPages = result.TotalPages
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }


        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
        {
            var createdDto = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
