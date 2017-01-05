using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApp.Interfaces;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
//proba
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
                //TODO: writter repository
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var pisac = new Writer(m.FirstNameWritter, m.LastNameWritter, DateTime.Now, Guid.Parse(currentUser.Id));
                var item = new Book(m.Text, pisac, Guid.Parse(currentUser.Id));
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
