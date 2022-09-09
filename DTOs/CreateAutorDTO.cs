using System.ComponentModel.DataAnnotations;
using WebApiAutors.Validations;

namespace WebApiAutors.DTOs
{
    public class CreateAutorDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        [FirstLetterCapital] 
        public string Name { get; set; }
    }
}
