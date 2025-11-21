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
        public IActionResult Get()
        {
            try
            {
                var categorias = _context.Categorias?.AsNoTracking().ToList();
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
        public IActionResult Get(int id)
        {
            var categoria = _context.Categorias?.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound($"Categoria com ID {id} não encontrada.");
            }
            return Ok(categoria);

        }

        [HttpGet("ObterProdutosCategoria")]
        public ActionResult<IEnumerable<Categoria>> GetProdutosCategoria()
        {
            var categorias = _context.Categorias?.Include(c => c.Produtos).AsNoTracking().ToList();

            if (categorias == null || categorias.Count == 0)
            {
                return NotFound("Nenhuma categoria encontrada.");
            }

            return Ok(categorias);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest("Categoria inválida.");
            }
            _context.Categorias?.Add(categoria);
            _context.SaveChanges();

            return CreatedAtAction("ObterCategoria", new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest("ID da categoria inválido.");
            }
            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var categoria = _context.Categorias?.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound($"Categoria com ID {id} não encontrada.");
            }

            _context.Categorias?.Remove(categoria);
            _context.SaveChanges();

            return Ok($"Categoria com ID {id} deletada com sucesso.");
        }
    }
}
