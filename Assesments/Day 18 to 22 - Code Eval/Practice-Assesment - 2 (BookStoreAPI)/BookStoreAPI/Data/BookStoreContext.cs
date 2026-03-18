using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data
{
    
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Bio).HasMaxLength(500);
            });

            
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.ISBN).HasMaxLength(13);

                
                entity.HasOne(b => b.Author)
                      .WithMany(a => a.Books)
                      .HasForeignKey(b => b.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "George Orwell",    Bio = "English novelist and essayist, journalist and critic." },
                new Author { Id = 2, Name = "J.K. Rowling",     Bio = "British author best known for the Harry Potter series." },
                new Author { Id = 3, Name = "F. Scott Fitzgerald", Bio = "American novelist of the Jazz Age." }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984",                     PublicationYear = 1949, ISBN = "9780451524935", AuthorId = 1 },
                new Book { Id = 2, Title = "Animal Farm",              PublicationYear = 1945, ISBN = "9780451526342", AuthorId = 1 },
                new Book { Id = 3, Title = "Harry Potter and the Philosopher's Stone", PublicationYear = 1997, ISBN = "9780747532699", AuthorId = 2 },
                new Book { Id = 4, Title = "Harry Potter and the Chamber of Secrets",  PublicationYear = 1998, ISBN = "9780747538493", AuthorId = 2 },
                new Book { Id = 5, Title = "The Great Gatsby",          PublicationYear = 1925, ISBN = "9780743273565", AuthorId = 3 }
            );
        }
    }
}
