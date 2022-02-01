using System;
using System.ComponentModel.DataAnnotations;

namespace CRUD_ORM.Models
{
    public class EmprestimoModel
    {
        public int Id { get; set; }
        public int LivroId { get; set; }
        public LivroModel Livro { get; set; }
        public int ClienteId { get; set; }
        public ClienteModel Cliente { get; set; }
        [DataType(DataType.Date)]
        public DateTime Emprestado { get; set; }
        [DataType(DataType.Date)]
        public DateTime PrevisaoDevolucao { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Devolucao { get; set; }
    }
}
