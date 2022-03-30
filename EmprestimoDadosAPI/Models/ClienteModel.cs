using System;
using System.ComponentModel.DataAnnotations;

namespace EmprestimoDadosAPI.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [DataType(DataType.Date)]
        public DateTime Nascimento { get; set; }
        [DataType(DataType.Date)]
        public DateTime Cadastro { get; set; }
        public bool Ativo { get; set; }
    }
}
