using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApp.Models;

namespace LibraryWebApp.Interfaces
{
    public interface IBookRepository
    {

        Book Get(Guid bookId);

        void Add(Book bookItem);
 
        bool Remove(Guid bookId, Guid userId);

        void Update(Book book, Guid userId);

        bool Posudi(Guid bookId, Guid userId, string username);

        bool Kupi(Guid bookId, Guid userId, string username);

        List<Posudba> GetAllFromCart(Guid userId);

        void Produzi(Guid bookId, Guid userId);

        List<Posudba> MojePosubeList(Guid userId);

        void Edit(Book book, Guid userId);

        List<Book> GetAllUserBooks(Guid userId);
        
        List<Book> GetAllBooks();

        List<Writer> GetAllWriters();

        Writer GetWriter(Guid writerId);

        void UpdatePosudba(Posudba temp);

        void RemoveFromCart(Guid Id);

        List<Posudba> GetForMailman();


        //List<Book> GetFiltered(Func<Book, bool> filterFunction, Guid userId);
    }
}
