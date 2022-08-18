using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutors.Validations;

namespace WebApiAutors.Entities
{
    public class Autor : IValidatableObject //IValidatableObject for model validation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        //[FirstLetterCapital] //custom validation
        public string Name { get; set; }

        [Range(18, 99)]
        [NotMapped]
        public int Age { get; set; }
        public List<Book> Books { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) //interface for model validation
        {
            if (!string.IsNullOrEmpty(Name)) 
            {
                var firstLetter = Name[0].ToString(); //[0] first char

                if (firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula", 
                        new string[] {nameof(Name)});
                }
            }
        }
    }
}
