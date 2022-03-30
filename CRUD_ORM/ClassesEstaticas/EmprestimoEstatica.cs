using CRUD_ORM.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRUD_ORM.ClassesEstaticas
{
    public class EmprestimoEstatica : EmprestimoModel
    {
        

        public static implicit operator string(EmprestimoEstatica emprestimo) =>
            $"{emprestimo.Id},{emprestimo.LivroId},{emprestimo.Livro}," +
            $"{emprestimo.ClienteId},{emprestimo.Cliente},{emprestimo.Emprestado}," +
            $"{emprestimo.PrevisaoDevolucao},{emprestimo.Devolucao}";
    }
}
