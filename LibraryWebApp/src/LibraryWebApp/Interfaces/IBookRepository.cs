﻿using System;
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

        void Posudi(Guid bookId, Guid userId, string username);



        //dohvati sve userove posuđene knjige
        List<Book> GetAllUserBooks(Guid userId);
        
        List<Book> GetAllBooks();


        //List<Book> GetFiltered(Func<Book, bool> filterFunction, Guid userId);
    }
}
