using Microsoft.AspNetCore.Mvc;
using SampleManagers;
using SampleModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookManager _bookManager;

        public BooksController(BookManager bookManager)
        {
            _bookManager = bookManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            var books = await _bookManager.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await _bookManager.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Book book)
        {
            await _bookManager.AddAsync(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var existingBook = await _bookManager.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            await _bookManager.UpdateAsync(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var book = await _bookManager.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookManager.DeleteAsync(id);
            return NoContent();
        }
    }
}