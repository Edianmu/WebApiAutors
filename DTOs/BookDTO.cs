using System.ComponentModel.DataAnnotations;
using WebApiAutors.Validations;

namespace WebApiAutors.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public List<AutorDTO> Autors { get; set; } //relation without DTO inheritance (see BookDTOWithAutors)
        //public List<CommentDTO> Comments { get; set; } 
    }
}
