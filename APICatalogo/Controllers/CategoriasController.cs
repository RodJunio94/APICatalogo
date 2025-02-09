using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }  

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        return _context.Categorias.Include(x => x.Produtos).AsNoTracking().ToList();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        try
        {
            return _context.Categorias.AsNoTracking().ToList();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados");
        }
        
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        try
        {
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.Id == id);

            if (categoria is null)
                return NotFound("Categoria não encontrada");

            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter a categoria do banco de dados");
        }        
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest();

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.Id)
            return BadRequest();

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> Delete(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);

        if (categoria is null)
            return NotFound();

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return Ok(categoria);
    }
}

