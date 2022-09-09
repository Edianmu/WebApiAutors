using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutors.DTOs;
using WebApiAutors.Entities;

namespace WebApiAutors.Controllers
{
    [ApiController]
    [Route("api/books/{bookId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
        {
            var existBook = await context.Books.AnyAsync(bookDB => bookDB.Id == bookId);
            if (!existBook)
            {
                return NotFound();
            }

            var comments = await context.Comments.Where(commentDB => commentDB.BookId == bookId).ToListAsync();

            return mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id:int}", Name = "getComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
                       
            if (comment == null)
            {
                return NotFound();
            }

            return mapper.Map<CommentDTO>(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Post(int bookId, CreateCommentDTO createCommentDTO)
        {
            var existBook = await context.Books.AnyAsync(bookDB => bookDB.Id == bookId);
            if (!existBook)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(createCommentDTO);
            comment.BookId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();

            var commentDTO = mapper.Map<CommentDTO>(comment);
            return CreatedAtRoute("getComment", new { id = comment.Id, bookId = bookId }, commentDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int bookId, int id, CreateCommentDTO createCommentDTO)
        {
            var existBook = await context.Books.AnyAsync(bookDB => bookDB.Id == bookId);
            if (!existBook)
            {
                return NotFound();
            }

            var existComment = await context.Comments.AnyAsync(commentDB => commentDB.Id == id);
            if (!existComment)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(createCommentDTO);
            comment.Id = id;
            comment.BookId = bookId;

            context.Update(comment);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
