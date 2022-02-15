using System.ComponentModel.DataAnnotations;

namespace CRUD_ORM.Models
{
    public class EmprestimoExistente : EmprestimoModel
    {
        public bool EmprestimoExiste { get; set; }
    }
}
