using CRUD_ORM.Controllers;
using CRUD_ORM.Data;
using CRUD_ORM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitBibliotecaTest
{
    public class Action_LivroTest
    {
        private readonly ClienteContext _context;
        public Action_LivroTest()
        {
            var db = new DbContextOptionsBuilder<ClienteContext>().UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=LivrariaApp;Data Source=DESKTOP-0GGDBVO\\SQLEXPRESS");
            _context = new ClienteContext(db.Options);
        }
        [Fact]
        public void ChamadaActionObterPrimeiroLivro()
        {
            LivroModel livro = _context.Livros.Include(c => c.Categoria).First();
            LivroModelsController controller = new LivroModelsController(_context);

            ViewResult livroDetail = controller.Details(livro.Id).Result as ViewResult;

            Assert.NotNull(livroDetail);
            Assert.NotNull(livroDetail.Model);
            Assert.IsType<LivroModel>(livroDetail.Model);

            Assert.Equal(livroDetail.Model, livro);
        }
    }
}
