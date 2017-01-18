using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{

    public enum Genres
    {
        Fiction,
        Comedy,
        Drama,
        Horror,
        NonFiction,
        RealisticFiction,
        SciFi,

    }

    public class Book
    {
        //knjiznicar pri unosu nove knjige
        public string Title { get; set; }
        public Guid BookId { get; set; }
        public Writer Writer { get; set; }
        public List<Posudba> Posudbe { get; set; }

        public int BorrowCounter { get; set; }
        public int SaleCounter { get; set; }
        public bool ZaKupnju { get; set; }
        public bool ZaPosudbu { get; set; }
        public double Price { get; set; }
        public Genres Genre { get; set; }

        //dinamicki unosi
        public string About { get; set; }
        //broj knjiga 
        public string ImgPath { get; set; }

        public Guid UserId { get; set; }
        public Book()
        {
            
        }

        public Book(string title, Writer writer, Guid userId, string about, Genres genre,
            int salecounter, int borrowcounter, bool zakupnju, bool zaposudbu, double price)
        {
            Title = title;
            Writer = writer;
            BookId = Guid.NewGuid();
            UserId = userId;
            Posudbe = new List<Posudba>();
            BorrowCounter = borrowcounter;
            SaleCounter = salecounter;
            About = about;
            ZaKupnju = zakupnju;
            ZaPosudbu = zaposudbu;
            Price = price;
            Genre = genre;
        }


        public bool IsAvailableForBorrowing()
        {
            if (BorrowCounter > 0) return true;

            return false;
        }

        public bool IsAvailableForSale()
        {
            if (SaleCounter > 0) return true;

            return false;
        }


        //ovo je jednako
        public override bool Equals(object obj)
        {
            var newObj = obj as Book;

            if (newObj == null)
            {
                return false;
            }
            else
            {
                return BookId.Equals(newObj.BookId);
            }

        }

        public override int GetHashCode()
        {
            return BookId.GetHashCode();
        }


    }
}
