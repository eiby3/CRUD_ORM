using CRUD_ORM.Data;
using CRUD_ORM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace xUnitBibliotecaTest
{
    public class EF_LivroTest
    {
        private readonly ClienteContext _context;
        public EF_LivroTest()
        {
            var db = new DbContextOptionsBuilder<ClienteContext>().UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=LivrariaApp;Data Source=DESKTOP-0GGDBVO\\SQLEXPRESS");
            _context = new ClienteContext(db.Options);
            

        }
        [Fact]
        public void ObterPrimeiroLivroCadastradoNoBD()
        {
            LivroModel livro = _context.Livros.Include(c => c.Categoria).First();

            Assert.NotNull(livro);
            Assert.NotNull(livro.Nome);
            Assert.True(livro.Ativo);
            Assert.NotNull(livro.Autor);
            Assert.NotNull(livro.Categoria);
            Assert.NotNull(livro.Categoria.Nome);
            Assert.True(livro.Categoria.Ativo);
        }
    }
}
