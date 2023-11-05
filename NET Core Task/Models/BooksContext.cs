using Microsoft.EntityFrameworkCore;

namespace NET_Core_Task.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
    }

}
