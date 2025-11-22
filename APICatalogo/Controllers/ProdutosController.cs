using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        var produtos = await _context.Produtos?.AsNoTracking().ToListAsync();
        if (produtos is null)        
            return NotFound("produtos não encontrados!");
        
        return produtos;
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> Get(int id)
    {
        var produto = await _context.Produtos?.FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null)
            return NotFound("produto não encontrado!");

        return produto;
    }

    [HttpPost]
    public async Task<ActionResult> Post(Produto produto)
    {
        if (produto is null)
            return BadRequest();

        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, Produto produto)
    {
        if (id != produto.Id)
            return BadRequest("");

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var produto = await _context.Produtos?.FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null)
            return NotFound("Produto não encontrado");

        _context.Produtos?.Remove(produto);
        await _context.SaveChangesAsync();

        return Ok(produto);
    }

}
