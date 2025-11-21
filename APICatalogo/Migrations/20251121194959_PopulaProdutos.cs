using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                   "VALUES ('Coca-cola', 'Coca-cola 700ml', 29.99, 'camiseta.jpg', 100, NOW(), 1)");
            mb.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                     "VALUES ('Pepsi', 'Pepsi 700ml', 27.99, 'camiseta.jpg', 100, NOW(), 1)");
            mb.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                        "VALUES ('Guaraná Antarctica', 'Guaraná Antarctica 700ml', 25.99, 'camiseta.jpg', 100, NOW(), 1)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Produtos WHERE Nome IN ('Coca-cola', 'Pepsi', 'Guaraná Antarctica')");
        }
    }
}
