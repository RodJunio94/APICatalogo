using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext appDbContext) : base(appDbContext)
    {       
    }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(on => on.Id).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);
        
        return produtosOrdenados;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltro)
    {
        var produtos = GetAll().OrderBy(on => on.Id).AsQueryable();

        if (produtosFiltro.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltro.PrecoCriterio))
        {
            if (produtosFiltro.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > produtosFiltro.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltro.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < produtosFiltro.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltro.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtosFiltro.Preco.Value).OrderBy(p => p.Preco);
            }
        }
        
        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltro.PageNumber, produtosFiltro.PageSize);
        
        return produtosFiltrados;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(p => p.CategoriaId == id).ToList();
    }
}
    
