using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CRUD_ORM.logs.livros
{
    public class LivroUpdateModel : LivroModelBase
    {
        public string NomeOriginal { get; set; }
        public string NomeAlterado { get; set; }
        public string AutorOriginal { get; set; }
        public string AutorAlterado { get; set; }
        public string CategoriaOriginal { get; set; }
        public string CategoriaAlterada { get; set; }
        public bool AtivoOriginal { get; set; }
        public bool AtivoAlterado { get; set; }
    }
}
