using EmprestimoDadosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace EmprestimoDadosAPI.Controllers
{
    public class EmprestimoController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmprestimoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("Emprestimo/{id}")]
        public IActionResult ExportarDados(int? id)
        {
            var emprestimo = GetEmprestimoById(id);

            if(emprestimo == null)
                return NotFound();

            return Ok(emprestimo);
        }
        private EmprestimoViewModel GetEmprestimoById(int? id)
        {
            EmprestimoViewModel emprestimo = new EmprestimoViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Connection")))
            {
                sqlConnection.Open();
                emprestimo = sqlConnection.Query<EmprestimoViewModel>("SELECT * FROM [Emprestimos] WHERE Id=@Id"
                                                         , new { Id = id }).FirstOrDefault();
                var cliente = sqlConnection.Query<ClienteModel>("SELECT * FROM [Clientes] WHERE Id=@Id"
                                                         , new { Id = emprestimo.ClienteId }).FirstOrDefault();
                var livro = sqlConnection.Query<LivroModel>("SELECT * FROM [Livros] WHERE Id=@Id"
                                                         , new { Id = emprestimo.LivroId }).FirstOrDefault();
                var categoria = sqlConnection.Query<CategoriaModel>("SELECT * FROM [Categorias] WHERE Id=@Id"
                                                         , new { Id = livro.CategoriaId }).FirstOrDefault();

                livro.Categoria = categoria;
                emprestimo.Livro = livro;
                emprestimo.Cliente = cliente;
            }
            return emprestimo;
        }
    }
}
