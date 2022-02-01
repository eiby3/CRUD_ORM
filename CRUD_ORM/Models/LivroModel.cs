using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_ORM.Models
{
    public class LivroModel
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CategoriaId { get; set; }        
        public CategoriaModel Categoria { get; set; }
        public string Autor { get; set; }
        public bool Ativo { get; set; }
    }
}
