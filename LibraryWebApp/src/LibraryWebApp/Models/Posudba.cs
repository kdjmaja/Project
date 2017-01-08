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

        public Posudba(Book book, Guid userId, string username)
        {
            Book = book;
            Title = book.Title;
            Writer = book.Writer;
            UserId = userId;
            PosudbaId = new Guid();
            DanPosudbe = DateTime.Now;
            DanVracanja = new DateTime(DanPosudbe.Day+30);
            Active = true;
            Username = username;
        }

        public void Produzi(Guid userId)
        {
            if (userId != UserId)
            {
                throw new AccessViolationException();
            }
            DanVracanja = new DateTime(DanVracanja.Day + 30);
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
