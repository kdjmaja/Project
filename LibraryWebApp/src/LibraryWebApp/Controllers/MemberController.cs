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
            bool uspjelo = _repository.Posudi(Id, Guid.Parse(currentUser.Id),currentUser.UserName);
            return RedirectToAction("MojePosudbe");


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
            List<Posudba> posudbe = _repository.MojePosubeList(Guid.Parse(currentUser.Id));
            posudbe = posudbe.OrderBy(p => p.DanVracanja.Date).ToList();
            return View(posudbe);
        }
        



    }
}