using Microsoft.EntityFrameworkCore;
using WebApiAutors.Entities;

namespace WebApiAutors
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Autor> Autors { get; set; } //Autors is the table name in DB
        public DbSet<Book> Books { get; set; }  //it creates tables with propieties from Autor and Book 
                                              
    }
}
