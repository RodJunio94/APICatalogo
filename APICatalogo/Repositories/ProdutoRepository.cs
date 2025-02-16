using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public IQueryable<Produto> GetProdutos()
    {
        return _context.Produtos;
    }

    public Produto GetProduto(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

        if (produto is null)
            throw new InvalidOperationException("O produto não existe");

        return produto;
    }

    public Produto Create(Produto produto)
    {
        if(produto is null)        
            throw new ArgumentNullException(nameof(produto));

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return produto;

    }

    public bool Update(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException(nameof(produto));

        if (_context.Produtos.Any(p => p.Id == produto.Id))
        {
            _context.Produtos.Update(produto);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
        
    public bool Delete(int id)
    {
        var produto = _context.Produtos.Find(id);

        if (produto is null)
            return false;

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return true;
    } 
}
    
