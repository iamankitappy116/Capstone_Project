using Microsoft.EntityFrameworkCore;
using WebAPI1.Models;

namespace WebAPI1.Data
{
    public class BookDbContext: DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options): base(options)
        {   
        }

        public DbSet<Book>Books { get; set; }
    }
}
