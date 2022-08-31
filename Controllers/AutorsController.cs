using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutors.Entities;
using WebApiAutors.Services;

namespace WebApiAutors.Controllers
{
    [ApiController]
    [Route("api/autors")]
    public class AutorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<AutorsController> logger;

        public AutorsController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<AutorsController> logger)
        {
            this.context = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        public ActionResult GetGuids()
        {
            return Ok(new
            {
                AutorsController_Transcient = serviceTransient.Guid,
                ServiceA_Transient = service.GetTransient(),
                AutorsController_Scoped = serviceScoped.Guid,
                ServiceA_Scoped = service.GetScoped(),
                AutorsController_Singleton = serviceSingleton.Guid,
                
                
                ServiceA_Singleton = service.GetSingleton()
            });
        }

        [HttpGet]
        [HttpGet("/list")]
        public async Task<ActionResult<List<Autor>>> Get() //Get is the class name
        {
            throw new NotImplementedException();
            logger.LogInformation("Estamos obteniendo la lista de autores");
            return await context.Autors.Include(x => x.Books).ToListAsync();   
        }

        [HttpGet("first")] //api/autors/first
        public async Task<ActionResult<Autor>> FirstAutor()
        {
            return await context.Autors.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")] //with parameter
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autors.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpGet("{name}")] //with parameter
        public async Task<ActionResult<Autor>> Get(string name)
        {
            var autor = await context.Autors.FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor) 
        {
            //validation
            var sameName = await context.Autors.AnyAsync(x => x.Name == autor.Name);
            if (sameName)
            {
                return BadRequest($"Ya existe un autor con el nombre {autor.Name}");
            }
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] //api/atours/1
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            var exist = await context.Autors.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
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
