using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutors.Validations;

namespace WebApiAutors.Entities
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        [FirstLetterCapital] //custom validation
        public string Name { get; set; }
        public List<AutorBook> AutorsBooks { get; set; }
    }
}
