using CRUD_ORM.Data;
using CRUD_ORM.logs.livros;
using CRUD_ORM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CRUD_ORM.logs
{
    public class livroLog
    {
        private readonly ClienteContext _context;
        private readonly IBibliotecaMongoSettings _mongoSettings;
        private readonly IMongoDatabase _database;
        public livroLog(ClienteContext context, IBibliotecaMongoSettings mongoSettings)
        {
            _mongoSettings = mongoSettings;
            _context = context;
            var client = new MongoClient(mongoSettings.ConnectionString);
            _database = client.GetDatabase(mongoSettings.Database);
            
        }
        public async Task Create(LivroModel livroModel)
        {
            var books = _database.GetCollection<LivroCreateModel>(_mongoSettings.CollectionName);
            var livro = new LivroCreateModel()
            {
                NomeAlterado = livroModel.Nome,
                AutorAlterado = livroModel.Autor,
                CategoriaAlterada = CategoriaNome(livroModel.Id).ToString(),
                AtivoAlterado = livroModel.Ativo,
                Data = DateTime.Now,
                Acao = "Create"
            };            
            await books.InsertOneAsync(livro);
        }
        public async Task Update(LivroModel livroModel, LivroModel livroAntigo)
        {
            var books = _database.GetCollection<LivroUpdateModel>(_mongoSettings.CollectionName);
            var livro = new LivroUpdateModel()
            {
                NomeOriginal = livroAntigo.Nome,
                NomeAlterado = livroModel.Nome,
                AutorOriginal = livroAntigo.Autor,
                AutorAlterado = livroModel.Autor,
                CategoriaOriginal = livroAntigo.Categoria.Nome,
                CategoriaAlterada = CategoriaNome(livroModel.Id).ToString(),
                AtivoOriginal = livroAntigo.Ativo,
                AtivoAlterado = livroModel.Ativo,
                Data = DateTime.Now,
                Acao = "Update"
            };
            await books.InsertOneAsync(livro);
        }
        public async Task Delete(LivroModel livroModel)
        {
            var books = _database.GetCollection<LivroDeleteModel>(_mongoSettings.CollectionName);
            var livro = new LivroDeleteModel()
            {
                NomeOriginal = livroModel.Nome,
                Data = DateTime.Now,
                Acao = "Delete"
            };
            await books.InsertOneAsync(livro);
        }

        private string CategoriaNome(int id)
        {
            var livroModel = _context.Livros
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            return livroModel.Result.Categoria.Nome;
        }
        
    }
}
