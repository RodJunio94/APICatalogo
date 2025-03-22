using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APICatalogo.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{   
    PagedList<Categoria> getCategorias(CategoriaParameters categoriaParameters);
    PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriaFiltroParams);
}
