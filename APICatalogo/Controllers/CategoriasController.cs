using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger)
    {
        _uof = unitOfWork;
        _logger = logger;
    }     

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _uof.CategoriaRepository.GetAll();

        if (categorias is null)
        {
            _logger.LogWarning($"Categorias não encontradas");
            return NotFound("Categorias não encontradas");
        }       

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriaParameters categoriaParameters)
    {
        var categorias = _uof.CategoriaRepository.getCategorias(categoriaParameters);

        return ObterCategorias(categorias);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };
        
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        
        var categoriasDto = categorias.ToCategoriaDTOList();
        
        return Ok(categoriasDto);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriaFiltradas([FromQuery] CategoriasFiltroNome categoriaFiltro)
    {
        var categoriasFiltradas = _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriaFiltro);
        
        return ObterCategorias(categoriasFiltradas);
        
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> GetById(int id)
    {
        var categoria = _uof.CategoriaRepository.GetById(c => c.Id == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Catergoria não encontrada");
            return NotFound("Categoria não encontrada");
        }
        
       var categoriasDto = categoria.ToCategoriaDTO();

        return Ok(categoriasDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest();

        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
        _uof.Commit();

        var novaCategoriaDTO = categoriaCriada.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDTO.Id }, novaCategoriaDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.Id)
            return BadRequest();

        var categoria = categoriaDto.ToCategoria();

        var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

       var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _uof.CategoriaRepository.GetById(c => c.Id == id);

        if (categoria is null)
            return NotFound();

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit();

        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluidaDto);
    }
}

