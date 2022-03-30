using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_ORM.Data;
using CRUD_ORM.Models;
using System.IO;
using System.Xml.Serialization;
using CRUD_ORM.ClassesEstaticas;
using System.Text;
using System.Data;
using System.ComponentModel;
using ServiceStack;

namespace CRUD_ORM.Controllers
{
    public class EmprestimoModelsController : Controller
    {
        private readonly ClienteContext _context;

        public EmprestimoModelsController(ClienteContext context)
        {
            _context = context;
        }

        // GET: EmprestimoModels
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clienteContext = _context.Emprestimos.Include(e => e.Cliente).Include(e => e.Livro);
            return View(await clienteContext.ToListAsync());
        }


        // GET: EmprestimoModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimoModel = await _context.Emprestimos
                .Include(e => e.Cliente)
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimoModel == null)
            {
                return NotFound();
            }

            return View(emprestimoModel);
        }

        // GET: EmprestimoModels/Create
        public IActionResult Create()
        {
            EmprestimoExistente emprestimoExistente = new EmprestimoExistente()
            {
                Emprestado = DateTime.Now,
                PrevisaoDevolucao = DateTime.Now.AddDays(5)
            };
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Nome");
            return View(emprestimoExistente);
        }

        // POST: EmprestimoModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LivroId,ClienteId,Emprestado,PrevisaoDevolucao,Devolucao")] EmprestimoModel emprestimoModel)
        {
            EmprestimoExistente emprestimoExistente = new EmprestimoExistente()
            {
                Id = emprestimoModel.Id,
                Cliente = emprestimoModel.Cliente,
                ClienteId = emprestimoModel.ClienteId,
                Devolucao = emprestimoModel.Devolucao,
                Emprestado = emprestimoModel.Emprestado,
                Livro = emprestimoModel.Livro,
                LivroId = emprestimoModel.LivroId,
                PrevisaoDevolucao = emprestimoModel.PrevisaoDevolucao,
                EmprestimoExiste = EmprestimoClienteExists(emprestimoModel)
            };
            
            if (ModelState.IsValid && !EmprestimoClienteExists(emprestimoModel))
            {
                _context.Add(emprestimoModel);
                
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", emprestimoModel.ClienteId);
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Nome", emprestimoModel.LivroId);

            //return Json("emprestimo existente");
            //return View(emprestimoExistente);            
            return RedirectToAction(nameof(Devolver), emprestimoModel);
        }
        [NonAction]
        public bool EmprestimoClienteExists(EmprestimoModel emprestimoModel)
        {
            var cliente = _context.Clientes.Find(emprestimoModel.ClienteId);
            //var emprestimo = _context.Emprestimos.Find(cliente.Id);
            var emprestimo = _context.Emprestimos.OrderByDescending(p => p.Devolucao);
            var emprestimoVerificar = emprestimo.Where(p => p.ClienteId == cliente.Id);
            if (emprestimoVerificar == null)
            {
                return false;
            }
            else if (emprestimoVerificar.FirstOrDefault(m => m.Devolucao == null) == null)
            {
                return false;
            }
            return true;
        }

        // GET: EmprestimoModels/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimoModel = await _context.Emprestimos.FindAsync(id);
            if (emprestimoModel == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", emprestimoModel.ClienteId);
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Nome", emprestimoModel.LivroId);
           
            return View(emprestimoModel);
        }
        public IActionResult Devolver(EmprestimoModel emprestimoModel)
        {
            var cliente = _context.Clientes.Find(emprestimoModel.ClienteId);
            //var emprestimo = _context.Emprestimos.Find(cliente.Id);
            var emprestimo = _context.Emprestimos.OrderByDescending(p => p.Devolucao);
            var emprestimoVerificar = emprestimo.Where(p => p.ClienteId == cliente.Id);
            var emprestimoSemDevolucao = emprestimoVerificar.FirstOrDefault(x => x.Devolucao == null);

            return View(emprestimoSemDevolucao);
        }
        public async Task<IActionResult> DevolverLivro(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var emprestimo = GetEmprestimo(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            emprestimo.Devolucao = DateTime.Now.Date;
            _context.Update(emprestimo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: EmprestimoModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LivroId,ClienteId,Emprestado,PrevisaoDevolucao,Devolucao")] EmprestimoModel emprestimoModel)
        {
            if (id != emprestimoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprestimoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprestimoModelExists(emprestimoModel.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", emprestimoModel.ClienteId);
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Nome", emprestimoModel.LivroId);
            return View(emprestimoModel);
        }

        // GET: EmprestimoModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimoModel = await _context.Emprestimos
                .Include(e => e.Cliente)
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimoModel == null)
            {
                return NotFound();
            }

            return View(emprestimoModel);
        }

        // POST: EmprestimoModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprestimoModel = await _context.Emprestimos.FindAsync(id);
            _context.Emprestimos.Remove(emprestimoModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoModelExists(int id)
        {
            return _context.Emprestimos.Any(e => e.Id == id);
        }
        public IActionResult DownloadXML(int? id)
        {
            var arquivo = GetEmprestimo(id);
            var nomeTxt = $"{arquivo.Id.ToString()}.xml";
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "xml", nomeTxt);

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(ToXML(arquivo));
            }
            if (arquivo == null)
                return Content("filename not present");


            FileStream fileStream;
            try
            {
                fileStream = System.IO.File.OpenRead(path);
            }
            catch (DirectoryNotFoundException)
            {
                return new EmptyResult();
            }

            return File(fileStream, "text/csv", nomeTxt);
        }
        public IActionResult DownloadCSV(int? id)
        {
            var arquivo = GetEmprestimo(id);
            var nomeTxt = $"{arquivo.Id.ToString()}.csv";
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "csv", nomeTxt);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(ToCSV(arquivo));
            }
            if (arquivo == null)
                return Content("filename not present");


            FileStream fileStream;
            try
            {
                fileStream = System.IO.File.OpenRead(path);
            }
            catch (DirectoryNotFoundException)
            {
                return new EmptyResult();
            }

            return File(fileStream, "text/csv", nomeTxt);
        }
        private EmprestimoModel GetEmprestimo(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var emprestimo =  _context.Emprestimos
                .Include(e => e.Cliente)
                .Include(e => e.Livro)
                .FirstOrDefault(m => m.Id == id);
            if (emprestimo == null)
            {
                return null;
            }string teste = "teste";
            
            
            return emprestimo;
        }

        private string ToXML(EmprestimoModel emprestimoModel)
        {
            return emprestimoModel.ToXml();
        }
        private string ToCSV(EmprestimoModel emprestimoModel)
        {
            return emprestimoModel.ToCsv();
        }
        public IActionResult ExportarXML(int? id)
        {
            return Content(ToXML(GetEmprestimo(id)));
        }
        public IActionResult ExportarCSV(int? id)
        {
            return Content(ToCSV(GetEmprestimo(id)));
        }
       

    }
}
