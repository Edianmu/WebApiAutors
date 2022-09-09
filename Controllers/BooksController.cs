using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutors.DTOs;
using WebApiAutors.Entities;

namespace WebApiAutors.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<BookDTO>> Get() //Get is the class name
        {
            var books = await context.Books.ToListAsync();
            return mapper.Map<List<BookDTO>>(books);
        }

        [HttpGet("{id:int}", Name = "getBook")]
        public async Task<ActionResult<BookDTOWithAutors>> Get(int id)
        {
            var book = await context.Books
                .Include(bookDB => bookDB.AutorsBooks)
                .ThenInclude(autorBookDB => autorBookDB.Autor)
                .Include(bookBD => bookBD.Comments).FirstOrDefaultAsync(x => x.Id == id);
                       
            if (book == null)
            {
                return NotFound();
            }

            book.AutorsBooks = book.AutorsBooks.OrderBy(x => x.Order).ToList();

            return mapper.Map<BookDTOWithAutors>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateBookDTO createBookDTO)
        {
            //var existAutor = await context.Autors.AnyAsync(x => x.Id == book.AutorId);
            //if (!existAutor)
            //{
            //    return BadRequest($"No existe el autor del Id: {book.AutorId}");
            //}
            if(createBookDTO.AutorsIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores.");
            }

            var autorsIds = await context.Autors
                .Where(autorDB => createBookDTO.AutorsIds.Contains(autorDB.Id)) //Query
                .Select(x => x.Id).ToListAsync(); //Only catch Id

            if(createBookDTO.AutorsIds.Count != autorsIds.Count)
            {
                return BadRequest("No existe alguno de los autores ingresados");
            }

            var book = mapper.Map<Book>(createBookDTO);

            OrderAutors(book);

            context.Add(book);
            await context.SaveChangesAsync();

            var bookDTO = mapper.Map<BookDTO>(book);
            return CreatedAtRoute("getBook", new {id = book.Id}, bookDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CreateBookDTO createBookDTO)
        {
            var bookDB = await context.Books
              .Include(x => x.AutorsBooks)
              .FirstOrDefaultAsync(x => x.Id == id);

            if (bookDB == null)
            {
                return NotFound();
            }
            bookDB = mapper.Map(createBookDTO, bookDB);

            OrderAutors(bookDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        private void OrderAutors (Book book)
        {

            if (book.AutorsBooks != null) //autors order when post the book's autor id on Swagger
            {
                for (int i = 0; i < book.AutorsBooks.Count; i++)
                {
                    book.AutorsBooks[i].Order = i;
                }
            }
        }

    }
}
