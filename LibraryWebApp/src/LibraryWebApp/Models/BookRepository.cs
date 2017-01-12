using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApp.Interfaces;

namespace LibraryWebApp.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

        public Book Get(Guid bookId)
        {
            var item = _context.Books.Include(b => b.Writer).Include(p => p.Writer.WritersBooks).Include(b => b.Posudbe).FirstOrDefault(p => p.BookId.Equals(bookId));
            if (item == null) return null;
            return item;
        }

        public void Add(Book bookItem)
        {
            if(bookItem == null) throw new ArgumentNullException();
            if (_context.Books.Any(s => s.BookId.Equals(bookItem.BookId)))
                bookItem.Counter++;

            else
            {
                _context.Books.Add(bookItem);
            }
            _context.SaveChanges();
        }

        public void Edit(Book book, Guid userId)
        {
            var item = Get(book.BookId);
            _context.Books.Attach(item);

            item.Counter = book.Counter;
            item.About = book.About;
            item.Writer.FirstName = book.Writer.FirstName;
            item.Writer.LastName = book.Writer.LastName;

            List<Posudba> borrows = SvePosudbe().Where(p => p.Title == item.Title).ToList();
            for(var i=0; i < borrows.Count; i++)
            {
                var posudba = borrows.ElementAt(i);
                _context.Posudbe.Attach(posudba);
                posudba.Title = book.Title;
                posudba.Writer.FirstName = book.Writer.FirstName;
                posudba.Writer.LastName = book.Writer.LastName;
            }
            item.Title = book.Title;
            _context.SaveChanges();
        }

        public bool Remove(Guid bookId, Guid userId)
        {
            var item = Get(bookId);
            if (item == null) return false;
            if(item.Writer.WritersBooks.Count == 1)
            {
                _context.Writers.Remove(item.Writer);
            }
            _context.Books.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public bool Posudi(Guid bookId, Guid userId, string username)
        {
            var knjiga = Get(bookId);
            var borrow = new Posudba(knjiga, userId, username);
            if (knjiga.Posudbe == null)
            {
                knjiga.Posudbe = new List<Posudba>();
            }
            if (knjiga.Posudbe.FirstOrDefault(p => p.Username == username) == null ||
                knjiga.Posudbe.FirstOrDefault(p => p.Username == username).Active == false)
            {
                knjiga.Posudbe.Add(borrow);
                Update(knjiga, userId);
                return true;
            }
            return false;
        }

        public void Produzi(Guid bookId, Guid userId)
        {
            var knjiga = Get(bookId);
            if (knjiga != null && knjiga.Posudbe.Count>0)
            {
                var posudba = knjiga.Posudbe.FirstOrDefault(s => s.UserId == userId);
                if (posudba != null)
                {
                    posudba.DanVracanja = posudba.DanVracanja.AddMonths(1);
                    Update(knjiga, userId);
                }
            }
        }

        public void Update(Book book, Guid userId)
        {
            if(book == null) throw new NullReferenceException();

            var item = Get(book.BookId);
            if (item == null)
            {
                Add(book);
            }
            else
            {
                item.About = book.About;
                item.Counter = book.Counter;
                item.Posudbe = book.Posudbe.ToList();

            }
            _context.SaveChanges();

        }

        public List<Posudba> MojePosubeList(Guid userId)
        {
            var books = GetAllBooks();
            var posudene = books.Where(p => p.Posudbe.Count > 0);
            List<Posudba> posudbe = new List<Posudba>();
            foreach (var book in posudene)
            {
                foreach (var posudba in book.Posudbe)
                {
                    if (posudba.UserId == userId)
                    {
                        posudbe.Add(posudba);
                    }
                }
            }
            return posudbe;
        }
        public List<Posudba> SvePosudbe()
        {
            var books = GetAllBooks();
            var posudene = books.Where(p => p.Posudbe.Count > 0);
            List<Posudba> posudbe = new List<Posudba>();
            foreach (var book in posudene)
            {
                foreach (var posudba in book.Posudbe)
                {

                        posudbe.Add(posudba);

                }
            }
            return posudbe;
        }
        public List<Writer> GetAllWriters()
        {
            return _context.Writers.ToList();
        }

        public Writer GetWriter(Guid writerId)
        {
            var item = _context.Writers.Include(p => p.WritersBooks).FirstOrDefault(p => p.WriterId.Equals(writerId));
            if (item == null) return null;
            return item;
        }
        public List<Book> GetAllUserBooks(Guid userId)
        {
            return _context.Books.Where(s => s.UserId.Equals(userId)).OrderByDescending(s => s.Title).ToList();
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.Include(b => b.Writer).Include(b => b.Posudbe).OrderByDescending(p => p.Title).ToList();
        }
    }
}
