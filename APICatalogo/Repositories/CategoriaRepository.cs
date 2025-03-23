using System.ComponentModel;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{   
    public CategoriaRepository(AppDbContext appDbContext) : base(appDbContext)
    {        
    }

    public async Task<IPagedList<Categoria>> getCategoriasAsync(CategoriaParameters categoriaParams)
    {
        var categorias = await GetAllAsync();

        var categoriasOrdenadas = categorias.OrderBy(c => c.Id).AsQueryable();
        
        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);
        
        return categoriasFiltradas;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriaFiltroParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrWhiteSpace(categoriaFiltroParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriaFiltroParams.Nome));
        }
        
        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriaFiltroParams.PageNumber, categoriaFiltroParams.PageSize);

        return categoriasFiltradas;
    }
}
