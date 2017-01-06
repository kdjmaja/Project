using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{

    public enum Genre
    {
        Fiction,
        Comedy,
        Drama,
        Horror,
        NonFiction,
        RealisticFiction,

    }
    //najbolja klasa ikad
    public class Book
    {
        //knjiznicar pri unosu nove knjige
        public string Title { get; set; }
        public int Counter { get; set; }
        public Guid BookId { get; set; }
        public Writer Writer { get; set; }

        //dinamicki unosi
        public string About { get; set; }
        //broj knjiga 

        public Guid UserId { get; set; }
        public Book()
        {
            
        }
        public Book(string title, Writer writer, Guid userId)
        {
            Title = title;
            Writer = writer;
            BookId = Guid.NewGuid();
            UserId = userId;

        }


        public Boolean IsAvilable()
        {
            if (Counter > 0) return true;

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
