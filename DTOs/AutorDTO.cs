namespace WebApiAutors.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<BookDTO> Books { get; set; } //relation without DTO inheritance (see AutorDTOWithBooks)
    }
}
