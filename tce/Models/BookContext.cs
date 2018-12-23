﻿using Microsoft.EntityFrameworkCore;

namespace TceApi.Models
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
                .HasIndex(u => u.ISBN)
                .IsUnique();
        }

    }
}