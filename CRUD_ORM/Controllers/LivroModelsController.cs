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

namespace CRUD_ORM.Controllers
{
    public class LivroModelsController : Controller
    {
        private readonly ClienteContext _context;
        private readonly IConfiguration _configuration;


        public LivroModelsController(ClienteContext context)
        {
            _context = context;
        }

        // GET: LivroModels
        public async Task<IActionResult> Index()
        {
            var clienteContext = _context.Livros.Include(l => l.Categoria);
            LivroModel livro = _context.Livros.Include(c => c.Categoria).First();
            return View(await clienteContext.ToListAsync());
            
        }

        // GET: LivroModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

            return View(livroModel);
        }

        // GET: LivroModels/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        // POST: LivroModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CategoriaId,Autor,Ativo")] LivroModel livroModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livroModel);
                await _context.SaveChangesAsync();
               
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", livroModel.CategoriaId);
            return View(livroModel);
        }

        // GET: LivroModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
            livroAntigo.Categoria.Nome =  livroModel.Categoria.Nome;
            
            
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", livroModel.CategoriaId);
            return View(livroModel);
        }

        // POST: LivroModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CategoriaId,Autor,Ativo")] LivroModel livroModel)
        {
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

            return View(livroModel);
        }

        // POST: LivroModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livroModel = await _context.Livros.FindAsync(id);
            _context.Livros.Remove(livroModel);
            await _context.SaveChangesAsync();

           
            return RedirectToAction(nameof(Index));
        }

        private bool LivroModelExists(int id)
        {
            return _context.Livros.Any(e => e.Id == id);
        }
        

    }
}
