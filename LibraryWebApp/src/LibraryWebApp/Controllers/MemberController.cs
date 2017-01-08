using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApp.Interfaces;
using LibraryWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers
{
    

    [Authorize(Policy = "RequireMemberRole")]
    public class MemberController : Controller
    {
        private readonly IBookRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(IBookRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.User.Identity.GetUserId());
            var item = _repository.GetAllBooks();
            item.Reverse();
            return View(item);
        }

        public IActionResult AktivnePosudbe()
        {
            var item = _repository.GetAllBooks();
            var posudivane = item.Where(p => p.Posudbe!=null).ToList();
            List<Posudba> posudbe = new List<Posudba>();
            foreach (Book book in posudivane)
            {
                foreach (Posudba posudba in book.Posudbe)
                {
                    if (posudba.Active)
                    {
                        posudbe.Add(posudba);
                    }
                }
            }
            return View(posudbe);
        }

        public IActionResult Posudivanje(BookViewModel m)
        {
            return View();
        }
    }
}