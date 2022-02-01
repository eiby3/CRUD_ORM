using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CRUD_ORM.Models
{
    public class LivroLogModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LogId { get; set; }
        public string NomeOriginal { get; set; }
        public string NomeAlterado { get; set; } 
        public string AutorOriginal { get; set; }
        public string AutorAlterado { get; set; }
        public string CategoriaOriginal { get; set; } 
        public string CategoriaAlterada { get; set; }
        public bool AtivoOriginal { get; set; }
        public bool AtivoAlterado { get; set; }
        public DateTime Data { get; set; }
        public string Acao { get; set; }
    }
}
