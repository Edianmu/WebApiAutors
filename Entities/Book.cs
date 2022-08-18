using WebApiAutors.Validations;

namespace WebApiAutors.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [FirstLetterCapital]
        public string Title { get; set; }
        public int AutorId { get; set; }    
        public Autor Autor { get; set; }    
    }
}
