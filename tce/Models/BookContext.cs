using Microsoft.EntityFrameworkCore;

namespace tce.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>()
                .Property(u => u.ISBN)
                .IsRequired();

            builder.Entity<Book>()
                .HasIndex(u => u.ISBN)
                .IsUnique()
                .HasName("UK_Book_ISBN");

            builder.Entity<Book>()
                .Property(u => u.Name)
                .IsRequired();

            builder.Entity<Book>()
                .Property(u => u.Price)
                .HasDefaultValue(0);

            builder.Entity<Book>()
                .Property(u => u.Published)
                .HasDefaultValue(new System.DateTime(1971, 01, 01, 0, 0, 0));
          
        }

    }
}
