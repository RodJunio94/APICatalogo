using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using X.PagedList;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext appDbContext) : base(appDbContext)
    {       
    }

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var produtos = await GetAllAsync();
        var produtosOrdenados = produtos.OrderBy(on => on.Id).AsQueryable();

        var result = await produtosOrdenados.ToPagedListAsync(produtosParams.PageNumber, produtosParams.PageSize);
        return result;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltro)
    {
        var produtos = await GetAllAsync();
        var produtosOrdenados = produtos.OrderBy(on => on.Id).AsQueryable();

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
        
        var result = await produtosOrdenados.ToPagedListAsync(produtosFiltro.PageNumber, produtosFiltro.PageSize);
        return result;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();
        return produtos.Where(p => p.CategoriaId == id).ToList();
    }
}
    
