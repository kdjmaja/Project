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
        public IDbSet<Posudba> Posudbe { get; set; }
       // public IDbSet<Posudba> Cart { get; set; }

        public BookDbContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasKey(s => s.BookId);
            modelBuilder.Entity<Book>().Property(s => s.Title).IsRequired();
            modelBuilder.Entity<Book>().Property(s => s.Price).IsRequired();
            modelBuilder.Entity<Book>().Property(s => s.Genre).IsRequired();
            modelBuilder.Entity<Book>().HasMany(s => s.Posudbe).WithRequired(s => s.Book);
            modelBuilder.Entity<Book>().Property(s => s.ImgPath).IsOptional();

            modelBuilder.Entity<Book>().HasOptional(s => s.Writer).WithMany(p => p.WritersBooks);


            modelBuilder.Entity<Writer>().HasKey(s => s.WriterId);
            modelBuilder.Entity<Writer>().Property(s => s.FirstName);
            modelBuilder.Entity<Writer>().Property(s => s.LastName);

            modelBuilder.Entity<Posudba>().HasKey(s => s.PosudbaId);
            modelBuilder.Entity<Posudba>().Property(s => s.Title);
            modelBuilder.Entity<Posudba>().Property(s => s.DanPosudbe);
            modelBuilder.Entity<Posudba>().Property(s => s.DanVracanja);
            modelBuilder.Entity<Posudba>().Property(s => s.Username);
            modelBuilder.Entity<Posudba>().Property(s => s.ZaKupnju);



        }
    }
}
