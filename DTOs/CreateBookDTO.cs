using System.ComponentModel.DataAnnotations;
using WebApiAutors.Validations;

namespace WebApiAutors.DTOs
{
    public class CreateBookDTO
    {
        [FirstLetterCapital]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        public string Title { get; set; }
        public List<int> AutorsIds { get; set; }
    }
}
