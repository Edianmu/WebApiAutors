using Microsoft.EntityFrameworkCore;
using WebApiAutors.Entities;

namespace WebApiAutors
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutorBook>()
                .HasKey(ab => new { ab.AutorId, ab.BookId });
        }

        public DbSet<Autor> Autors { get; set; } //Autors is the table name in DB
        public DbSet<Book> Books { get; set; }  //it creates tables with propieties from Autor and Book 
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AutorBook> AutorBook { get; set; }                                           
    }
}
