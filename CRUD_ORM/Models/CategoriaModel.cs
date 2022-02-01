using System.ComponentModel.DataAnnotations;

namespace CRUD_ORM.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
    }
}
