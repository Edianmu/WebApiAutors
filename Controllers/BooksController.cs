using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutors.Entities;

namespace WebApiAutors.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            return await context.Books.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            var existAutor = await context.Autors.AnyAsync(x => x.Id == book.AutorId);
            if (!existAutor)
            {
                return BadRequest($"No existe el autor del Id: {book.AutorId}");
            }

            context.Add(book);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
