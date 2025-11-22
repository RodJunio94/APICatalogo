using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            try
            {
                var categorias = await _context.Categorias?.AsNoTracking().ToListAsync();
                if (categorias == null || categorias.Count == 0)
                {
                    return NotFound("Nenhuma categoria encontrada.");
                }

                return Ok(categorias);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
            
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult> Get(int id)
        {
            var categoria = await _context.Categorias?.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound($"Categoria com ID {id} não encontrada.");
            }
            return Ok(categoria);

        }

        [HttpGet("ObterProdutosCategoria")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetProdutosCategoria()
        {
            var categorias = await _context.Categorias?.Include(c => c.Produtos).AsNoTracking().ToListAsync();

            if (categorias == null || categorias.Count == 0)
            {
                return NotFound("Nenhuma categoria encontrada.");
            }

            return Ok(categorias);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest("Categoria inválida.");
            }
            _context.Categorias?.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("ObterCategoria", new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest("ID da categoria inválido.");
            }
            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _context.Categorias?.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound($"Categoria com ID {id} não encontrada.");
            }

            _context.Categorias?.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok($"Categoria com ID {id} deletada com sucesso.");
        }
    }
}
