using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltro);
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
    
}
