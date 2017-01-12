using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApp.Interfaces;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace LibraryWebApp.Controllers
{
    public class AdminController : Controller
    {

        private IBookRepository _repository;
        private UserManager<ApplicationUser> _userManager;

        public AdminController(IBookRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        
        public IActionResult Edit(Guid Id)
        {
            if (ModelState.IsValid)
            {
                var item = _repository.Get(Id);
                if (item != null)
                {
                    return View("Edit", item);
                }
            }
            return View("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("BookId","Title", "Counter", "About","UserId","Writer")]Book book)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Edit(book, Guid.Parse(currentUser.Id));
            return View("Index");
        }
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var item = _repository.GetAllBooks();
            item.Reverse();
            return View(item);
        }

        public IActionResult Add()
        {
            return View("Adder");

        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel m)
        {
            if (ModelState.IsValid)
            {
                
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                Book item;
                var pisac = new Writer(m.FirstNameWritter, m.LastNameWritter, DateTime.Now, Guid.Parse(currentUser.Id));
                var knjiga = _repository.GetAllBooks().FirstOrDefault(p => p.Writer.Equals(pisac));
                if (knjiga != null)
                {
                    Writer pisac1 = knjiga.Writer;
                    item = new Book(m.Text, pisac1, Guid.Parse(currentUser.Id),m.Counter,m.About);
                }
                else
                {
                    item = new Book(m.Text, pisac, Guid.Parse(currentUser.Id),m.Counter,m.About);
                }
                _repository.Add(item);
                return RedirectToAction("Index");
            }

            return View("Adder", m);
        }


        public async Task<IActionResult> DeleteBook(Guid Id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Remove(Id, Guid.Parse(currentUser.Id));
            return RedirectToAction("Index");

        }

        

    }
}
