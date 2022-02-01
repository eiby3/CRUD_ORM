using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUD_ORM.Migrations
{
    public partial class livrosNomeCategoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoriaNome",
                table: "Livros",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaNome",
                table: "Livros");
        }
    }
}
