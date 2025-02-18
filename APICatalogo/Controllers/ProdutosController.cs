using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;

    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("produtos/categoria/id")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
    {   
        var produtos = _repository.GetProdutosPorCategoria(id);
        if(produtos is null)
            return NotFound();

        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetAll();

        if(produtos is null)
            return NotFound();

        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> GetById(int id)
    {
        var produto = _repository.GetById(p => p.Id == id);

        if (produto is null)
            return NotFound("Produto não encontrado");

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if ( produto is null)
            return BadRequest();

        var novoProduto = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.Id }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if(id != produto.Id)
            return BadRequest();

        var produtoAtualizado = _repository.Update(produto);

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _repository.GetById(p => p.Id == id);
        if (produto is null)
            return NotFound("Produto não encontrado");

        var produtoDeletado = _repository.Delete(produto);
        return Ok(produtoDeletado);       
    }
}
