using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var item = _context.Books.Include(b => b.Writer).Include(b => b.Posudbe).FirstOrDefault(p => p.BookId.Equals(bookId));
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

        public bool Remove(Guid bookId, Guid userId)
        {
            var item = Get(bookId);
            if (item == null) return false;
            _context.Books.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Posudi(Guid bookId, Guid userId, string username)
        {
            var knjiga = Get(bookId);
            var borrow = new Posudba(knjiga, userId, username);
            if (knjiga.Posudbe == null)
            {
                knjiga.Posudbe = new List<Posudba>();
            }
            knjiga.Posudbe.Add(borrow);
            Update(knjiga, userId);
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
