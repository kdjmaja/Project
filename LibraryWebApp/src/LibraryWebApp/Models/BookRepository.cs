using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using LibraryWebApp.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;

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
            if (_context.Books.Any(s => s.BookId.Equals(bookItem.BookId) && s.ZaPosudbu == true))
                bookItem.BorrowCounter++;
            else if ((_context.Books.Any(s => s.BookId.Equals(bookItem.BookId) && s.ZaKupnju == true)))
                bookItem.SaleCounter++;
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

            item.BorrowCounter = book.BorrowCounter;
            item.SaleCounter = book.SaleCounter;
            item.About = book.About;
            var writers = GetAllWriters();
            foreach(var writer in writers)
            {
                if(writer.FirstName.Equals(book.Writer.FirstName) && writer.LastName.Equals(book.Writer.LastName) && !writer.WriterId.Equals(book.Writer.WriterId))
                {
                    item.Writer = writer;
                    RemoveWriter(book.Writer.WriterId);
                }
            }
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

        public bool RemoveWriter(Guid WriterId)
        {
            var item = GetWriter(WriterId);
            if (item == null) return false;
            _context.Writers.Remove(item);
            _context.SaveChanges();
            return true;
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

        public bool Kupi(Guid bookId, Guid userId, string username)
        {
            var knjiga = Get(bookId);
            var buy = new Posudba(knjiga, userId, username, true,true);
            if (knjiga.Posudbe == null)
            {
                knjiga.Posudbe = new List<Posudba>();
            }
            if (knjiga.Posudbe.FirstOrDefault(p => p.Username == username) == null ||
                knjiga.Posudbe.FirstOrDefault(p => p.Username == username).Active == false)
            {
                //prebacit kod prave posudbe-kosarica
                knjiga.Posudbe.Add(buy);
                knjiga.SaleCounter--;

                Update(knjiga, userId);

                _context.Posudbe.Add(buy);
               
                return true;
            }
            return false;
        }

        public bool Posudi(Guid bookId, Guid userId, string username)
        {
            var knjiga = Get(bookId);
            var borrow = new Posudba(knjiga, userId, username,false,true);
            if (knjiga.Posudbe == null)
            {
                knjiga.Posudbe = new List<Posudba>();
            }
            if (knjiga.Posudbe.FirstOrDefault(p => p.Username == username) == null ||
                knjiga.Posudbe.FirstOrDefault(p => p.Username == username).Active == false)
            {   
                //prebacit u kosaricu
                knjiga.Posudbe.Add(borrow);
                knjiga.BorrowCounter--;

                Update(knjiga, userId);

                _context.Posudbe.Add(borrow);
                
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
                item.BorrowCounter = book.BorrowCounter;
                item.SaleCounter = book.SaleCounter;
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

        //mozda maknuti userid
        public List<Posudba> GetAllFromCart(Guid userId)
        {
            return _context.Posudbe.Where(s => s.UserId.Equals(userId)).Include(b => b.Book).Where(c => c.ZaCart).Include(b => b.Writer).OrderByDescending(p => p.Title).ToList();
        }

        public void RemoveFromCart(Guid Id)
        {
            var item = _context.Posudbe.Include(p => p.Book).FirstOrDefault(p => p.PosudbaId.Equals(Id));
            var book = Get(item.Book.BookId);
            if (item.ZaKupnju)
            {
                book.SaleCounter++;
            }
            else
            {
                book.BorrowCounter++;
            }
            Update(item.Book, item.UserId);
            _context.Posudbe.Remove(item);
            _context.SaveChanges();

        }

        public List<Posudba> GetForMailman()
        {
            return _context.Posudbe.Include(p => p.Book).Where(s => s.ZaDostaviti && !s.Active).ToList();     
        }


        public void UpdatePosudba(Posudba temp)
        {
            if (temp == null) throw new NullReferenceException();

            var item = _context.Posudbe.FirstOrDefault(p => p.PosudbaId.Equals(temp.PosudbaId));
            if (temp == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                item.Active = temp.Active;
                item.ZaDostaviti = temp.ZaDostaviti;
                item.Adresa = temp.Adresa;
                item.DanPosudbe = temp.DanPosudbe;
                item.ZaCart = temp.ZaCart;

            }
            _context.SaveChanges();

        }


    }
}
