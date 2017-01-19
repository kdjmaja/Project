using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class Posudba
    {
        public Book Book { get; set; }
        public string Title { get; set; }
        public Writer Writer { get; set; }
        public Guid PosudbaId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DanPosudbe { get; set; }
        public DateTime DanVracanja { get; set; }
        public bool Active { get; set; }
        public string Username { get; set; }
        public bool ZaKupnju { get; set; }
        public bool ZaCart { get; set; }
        public bool ZaDostaviti { get; set; }
        public string Adresa { get; set; }
        public string ImeKupca { get; set; }
        public string PrezimeKupca { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; }

        public Posudba()
        {
            
        }
        public Posudba(Book book, Guid userId, string username,bool zakupnju,bool zacart)
        {
            Book = book;
            Title = book.Title;
            Writer = book.Writer;
            UserId = userId;
            PosudbaId = Guid.NewGuid();
            DanPosudbe = DateTime.Now;
            DanVracanja = DanPosudbe.AddMonths(1);
            Active = false;
            Username = username;
            ZaKupnju = zakupnju;
            ZaCart = zacart;
            ZaDostaviti = false;
            Quantity = 1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var newObj = obj as Posudba;
            return PosudbaId.Equals(newObj.PosudbaId);
        }

        public override int GetHashCode()
        {
            return PosudbaId.GetHashCode();
        }
    }

}
