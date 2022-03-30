using CRUD_ORM.logs.livros;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CRUD_ORM.logs
{
    public class LivroCreateModel : LivroModelBase
    {
        public string NomeAlterado { get; set; }
        public string AutorAlterado { get; set; }
        public string CategoriaAlterada { get; set; }
        public bool AtivoAlterado { get; set; }
    }
}
