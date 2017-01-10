using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApp.Interfaces;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers
{
    
    public class MemberController : Controller
    {
        private readonly IBookRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(IBookRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var item = _repository.GetAllBooks();
            item.Reverse();
            return View(item);
        }

        public async Task<IActionResult> BorrowBook(Guid Id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Posudi(Id, Guid.Parse(currentUser.Id),currentUser.UserName);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Produzi(Guid Id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Produzi(Id, Guid.Parse(currentUser.Id));
            return RedirectToAction("MojePosudbe");

        }

        public async Task<IActionResult> MojePosudbe()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var books = _repository.GetAllBooks();
            var posudene = books.Where(p => p.Posudbe.Count > 0);
            List<Posudba> posudbe = new List<Posudba>();
            foreach (var book in posudene)
            {
                foreach (var posudba in book.Posudbe)
                {
                    if (posudba.UserId == Guid.Parse(currentUser.Id))
                    {
                        posudbe.Add(posudba);
                    }
                }
            }
            return View(posudbe);
        }
        



    }
}