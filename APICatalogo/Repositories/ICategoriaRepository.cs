using APICatalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APICatalogo.Repositories;

public interface ICategoriaRepository
{
    IEnumerable<Categoria> GetCategorias();
    Categoria GetCategoria(int categoriaId);
    Categoria Create(Categoria categoria);
    Categoria Update(Categoria categoria);
    Categoria Delete(int categoriaId);
}
