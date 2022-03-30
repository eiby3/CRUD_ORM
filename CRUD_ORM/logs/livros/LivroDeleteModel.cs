using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CRUD_ORM.logs.livros
{
    public class LivroDeleteModel : LivroModelBase
    {
        public string NomeOriginal { get; set; }
    }
}
