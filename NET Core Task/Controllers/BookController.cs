using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NET_Core_Task.Models;

namespace NET_Core_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookContext _dbContext;

        public BookController(BookContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            if (_dbContext.Books == null)
            {
                return NotFound();
            }
            return await _dbContext.Books.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBooks(int id)
        {
            if (_dbContext.Books == null)
            {
                return NotFound();
            }
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooks), new { id = book.ID }, book);
        }

        [HttpPut]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(book).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
            private bool BookAvailable(int id)
            {
                return (_dbContext.Books?.Any(x => x.ID == id)).GetValueOrDefault();
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if(_dbContext.Books == null)
            {
                return NotFound();
            }
            var book = await _dbContext.Books.FindAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            _dbContext.Books.Remove(book);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
