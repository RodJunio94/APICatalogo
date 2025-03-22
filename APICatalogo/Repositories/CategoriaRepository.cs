using System.ComponentModel;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{   
    public CategoriaRepository(AppDbContext appDbContext) : base(appDbContext)
    {        
    }

    public PagedList<Categoria> getCategorias(CategoriaParameters categoriaParams)
    {
        var categorias = GetAll().OrderBy(c => c.Id).AsQueryable();
        
        var categoriasPaginadas = PagedList<Categoria>.ToPagedList(categorias, categoriaParams.PageNumber, categoriaParams.PageSize);
        
        return categoriasPaginadas;
    }

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriaFiltroParams)
    {
        var categorias = GetAll().AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoriaFiltroParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriaFiltroParams.Nome));
        }
        
        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriaFiltroParams.PageNumber, categoriaFiltroParams.PageSize);
        
        return categoriasFiltradas;
    }
}
