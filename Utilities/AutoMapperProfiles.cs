using AutoMapper;
using WebApiAutors.DTOs;
using WebApiAutors.Entities;

namespace WebApiAutors.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateAutorDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOWithBooks>()
                .ForMember(autorDTO => autorDTO.Books, options => options.MapFrom(MapAutorDTOBooks));

            CreateMap<CreateBookDTO, Book>()
                .ForMember(book => book.AutorsBooks, options => options.MapFrom(MapAutorsBooks));
            CreateMap<Book, BookDTO>();
            CreateMap<Book, BookDTOWithAutors>()
                .ForMember(bookDTO => bookDTO.Autors, options => options.MapFrom(MapBookDTOAutors));

            CreateMap<CreateCommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }

        private List<BookDTO> MapAutorDTOBooks(Autor autor, AutorDTO autorDTO)
        {
            var result = new List<BookDTO>();

            if (autor.AutorsBooks == null) { return result; }

            foreach (var autorBook in autor.AutorsBooks)
            {
                result.Add(new BookDTO()
                {
                    Id = autorBook.BookId,
                    Title = autorBook.Book.Title
                });
            }
            return result;
        }

        private List<AutorBook> MapAutorsBooks(CreateBookDTO createBookDTO, Book book) 
        {
            var result = new List<AutorBook>();

            if (createBookDTO.AutorsIds == null) { return result; }

            foreach (var autorId in createBookDTO.AutorsIds)
            {
                result.Add(new AutorBook() { AutorId = autorId });
            }

            return result;
        }

        private List<AutorDTO> MapBookDTOAutors(Book book, BookDTO bookDTO)
        {
            var result = new List<AutorDTO>();

            if (book.AutorsBooks == null) { return result; }

            foreach (var autorbook in book.AutorsBooks)
            {
                result.Add(new AutorDTO()
                { 
                    Id = autorbook.AutorId,
                    Name = autorbook.Autor.Name
                });
            }

            return result;
        }
    }
}
