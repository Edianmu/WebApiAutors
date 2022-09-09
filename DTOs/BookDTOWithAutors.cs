namespace WebApiAutors.DTOs
{
    public class BookDTOWithAutors : BookDTO
    {
        public List<AutorDTO> Autors { get; set; }
        public List<CommentDTO> Comments { get; set; }
    }
}
