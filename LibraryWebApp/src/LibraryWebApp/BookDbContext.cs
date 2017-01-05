using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryWebApp.Models;

namespace LibraryWebApp
{
    public class BookDbContext : DbContext
    {

        public IDbSet<Book> Books { get; set; }
        public IDbSet<Writer> Writers { get; set; }

        public BookDbContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasKey(s => s.BookId);
            modelBuilder.Entity<Book>().Property(s => s.Title).IsRequired();
            
            modelBuilder.Entity<Book>().HasOptional(s => s.Writer).WithMany(p => p.WritersBooks);

            modelBuilder.Entity<Writer>().HasKey(s => s.WriterId);
            modelBuilder.Entity<Writer>().Property(s => s.FirstName);
            modelBuilder.Entity<Writer>().Property(s => s.LastName);
        }
    }
}
