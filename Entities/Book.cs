using System.ComponentModel.DataAnnotations;
using WebApiAutors.Validations;

namespace WebApiAutors.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [FirstLetterCapital]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        public string Title { get; set; }  
        public List <Comment> Comments { get; set; }
        public List<AutorBook> AutorsBooks { get; set; }
    }
}
