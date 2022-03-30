using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_ORM.Data;
using CRUD_ORM.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Data.SqlClient;
using Dapper;

namespace CRUD_ORM.Controllers
{
    public class LivroModelsController : Controller
    {
        private readonly ClienteContext _context;
        private readonly ILogger<LivroModelsController> _logger;

        public LivroModelsController(ClienteContext context, ILogger<LivroModelsController> logger)
        {
            _logger = logger;
            _context = context;

        }

        // GET: LivroModels
        public async Task<IActionResult> Index()
        {
            _logger.LogTrace("Entrando na index");

            var clienteContext = _context.Livros.Include(l => l.Categoria);
            LivroModel livro = _context.Livros.Include(c => c.Categoria).First();

            _logger.LogInformation("Recuperando livros do bd e listando...");
            return View(await clienteContext.ToListAsync());


        }

        // GET: LivroModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogTrace($"Entrando em detalhes do livro com id: {id}");
            if (id == null)
            {
                _logger.LogInformation("Não há nenhum registro desse livro.");
                return NotFound();
            }

            var livroModel = await _context.Livros
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livroModel == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Mostrando detalhes");
            return View(livroModel);
        }

        // GET: LivroModels/Create
        public IActionResult Create()
        {
            _logger.LogTrace("Entrando na pagina criar do livro...");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            _logger.LogInformation("Esperando preencher os campos...");
            return View();
        }

        // POST: LivroModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CategoriaId,Autor,Ativo")] LivroModel livroModel)
        {
            _logger.LogTrace($"Preenchendo o livro com as infos {livroModel.Id}, {livroModel.Nome}, {livroModel.Autor}");
            if (ModelState.IsValid)
            {
                _context.Add(livroModel);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Campos preenchidos");

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", livroModel.CategoriaId);

            return View(livroModel);
        }

        // GET: LivroModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogTrace($"Entrando na pagina para editar o livro com o id:{id}");
            if (id == null)
            {
                return NotFound();
            }

            var livroModel = await _context.Livros.FindAsync(id);
            if (livroModel == null)
            {
                return NotFound();
            }
            LivroModel livroAntigo = livroModel;
            livroModel = await _context.Livros
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            livroAntigo.Categoria.Nome = livroModel.Categoria.Nome;


            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", livroModel.CategoriaId);
            _logger.LogInformation("Campos esperando para serem editados.");
            return View(livroModel);
        }

        // POST: LivroModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CategoriaId,Autor,Ativo")] LivroModel livroModel)
        {
            _logger.LogTrace($"Editando o livro com o id:{id}");
            if (id != livroModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livroModel);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Campos editados.");


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroModelExists(livroModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", livroModel.CategoriaId);

            return View(livroModel);
        }

        // GET: LivroModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogTrace($"Entrando na pagina para deletar o livro com o id:{id}");
            if (id == null)
            {
                return NotFound();
            }

            var livroModel = await _context.Livros
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (livroModel == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Esperando confirmação para deletar o livro");
            return View(livroModel);
        }

        // POST: LivroModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogTrace($"Confirmando deletar o livro com o id:{id}");
            var livroModel = await _context.Livros.FindAsync(id);
            _context.Livros.Remove(livroModel);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Livro deletado...");
            return RedirectToAction(nameof(Index));
        }

        private bool LivroModelExists(int id)
        {
            _logger.LogTrace($"Verificando se o livro com id:{id} existe");
            return _context.Livros.Any(e => e.Id == id);
        }
        public IActionResult ExportarCSV(int? id)
        {
            return Content(ToCSV(GetLivro(id)));
        }
        private string ToCSV(LivroModel livro)
        {
            Importer();
            return livro.ToCsv();
        }
        private LivroModel GetLivro(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var livroModel = _context.Livros
                .Include(l => l.Categoria)
                .FirstOrDefault(m => m.Id == id);
            if (livroModel == null)
            {
                return null;
            }


            return livroModel;
        }
        private void Importer()
        {
            using (var reader = new StreamReader(@"D:\livros1.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                 var records = csv.GetRecords<LivroModel>();
                
                List<LivroModel> livros = records.ToList();
                livros.ForEach(livro =>
                 {
                     var categoriaModel = _context.Categorias.Find(livro.CategoriaId);
                     livro.Categoria = categoriaModel;
                 });
                _context.Livros.AddRange(livros);
                _context.SaveChanges();
            }
        }

    }
}
