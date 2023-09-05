using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> get(int id)
        {
            var list = await context.Libros.Include(x => x.Autor).Where(x => x.AutorId == id).ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existe_autor = await context.Autores.AnyAsync(x=>x.Id==libro.AutorId);

            if (!existe_autor)
            {
                return BadRequest($"No existe el autor con Id:{libro.AutorId}.");
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Libro>> Put(Libro libro, int id)
        {
            var exist = await context.Libros.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return BadRequest("El Id del libro no coincide con el de la URL.");
            }
            else 
            {
                context.Update(libro);
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete("id:int")]
        public async Task<ActionResult<Libro>> Delete(Libro libro,int id) 
        {
            var exist = await context.Libros.AnyAsync(y => y.Id == id);

            if (!exist) 
            {
                return BadRequest("El Id del libro no coincide con el de la URL."); 
            }

            context.Remove(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
