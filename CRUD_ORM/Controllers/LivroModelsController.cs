﻿using System;
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
using CRUD_ORM.logs;

namespace CRUD_ORM.Controllers
{
    public class LivroModelsController : Controller
    {
        private readonly ClienteContext _context;
        private readonly IConfiguration _configuration;
        private readonly livroLog _livroLog;

        public LivroModelsController(ClienteContext context, IConfiguration configuration, livroLog livroLog)
        {
            _context = context;
            _configuration = configuration;
            _livroLog = livroLog;
        }

        // GET: LivroModels
        public async Task<IActionResult> Index()
        {
            var clienteContext = _context.Livros.Include(l => l.Categoria);
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
               
                await _livroLog.Create(livroModel);
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
            
            AdicionarSessao(livroAntigo);
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

                    LivroModel livroAntigo = ResgatarDaSessao();
                    await _livroLog.Update(livroModel, livroAntigo);
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

            await _livroLog.Delete(livroModel);
            return RedirectToAction(nameof(Index));
        }

        private bool LivroModelExists(int id)
        {
            return _context.Livros.Any(e => e.Id == id);
        }
        private void AdicionarSessao(LivroModel livro)
        {
            LivroModel livroAntigo = livro;
            HttpContext.Session.SetarNaSessao("livro", livroAntigo);
        }
        private LivroModel ResgatarDaSessao()
        {
            LivroModel livro = HttpContext.Session.RecuperarDaSessao<LivroModel>("livro");            
            return livro;
        }

    }
}
