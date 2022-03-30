using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CRUD_ORM.logs.livros
{
    public class LivroModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LogId { get; set; }
        public DateTime Data { get; set; }
        public string Acao { get; set; }
    }
}