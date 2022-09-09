using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutors.DTOs;
using WebApiAutors.Entities;

namespace WebApiAutors.Controllers
{
    [ApiController]
    [Route("api/autors")]
    public class AutorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutorsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<AutorDTO>> Get() //Get is the class name
        {
            var autors = await context.Autors.ToListAsync(); //List Autors table 
            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpGet("{id:int}", Name = "getAutor")] //with parameter
        public async Task<ActionResult<AutorDTOWithBooks>> Get(int id)
        {
            var autor = await context.Autors
                .Include(autorDB => autorDB.AutorsBooks)
                .ThenInclude(autorBook => autorBook.Book)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOWithBooks>(autor);
        }

        [HttpGet("{name}")] //with parameter
        public async Task<ActionResult<List<AutorDTO>>> Get(string name)
        {
            var autors = await context.Autors.Where(autorBD => autorBD.Name.Contains(name)).ToListAsync();

            if (autors == null)
            {
                return NotFound();
            }

            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateAutorDTO createAutorDTO) 
        {
            //validation
            var sameName = await context.Autors.AnyAsync(x => x.Name == createAutorDTO.Name);
            if (sameName)
            {
                return BadRequest($"Ya existe un autor con el nombre {createAutorDTO.Name}");
            }

            var autor = mapper.Map<Autor>(createAutorDTO);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("getAutor", new { id = autor.Id}, autorDTO);
        }

        [HttpPut("{id:int}")] //api/atours/1
        public async Task<ActionResult> Put(CreateAutorDTO createAutorDTO, int id)
        {

            var exist = await context.Autors.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(createAutorDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")] //api/atours/1
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Autors.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }  

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
