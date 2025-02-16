using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _repositoy;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger)
    {
        _repositoy = repository;
        _logger = logger;
    }     

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        var categorias = _repositoy.GetCategorias();
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        var categoria = _repositoy.GetCategoria(id);

        if (categoria is null)
        {
            _logger.LogWarning($"Catergoria não encontrada");
            return NotFound("Categoria não encontrada");
        }
        
        return Ok(categoria);
    }


    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest();

        var vategoriaCriada = _repositoy.Create(categoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = vategoriaCriada.Id }, vategoriaCriada);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.Id)
            return BadRequest();

        _repositoy.Update(categoria);

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> Delete(int id)
    {
        var categoria = _repositoy.GetCategoria(id);

        if (categoria is null)
            return NotFound();

        var categoriaExcluida = _repositoy.Delete(id);

        return Ok(categoriaExcluida);
    }
}

