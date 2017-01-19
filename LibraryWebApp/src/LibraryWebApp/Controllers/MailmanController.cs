using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryWebApp.Controllers
{
    [Authorize(Policy="Mailmans")]
    public class MailmanController : Controller
    {
        private readonly IBookRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MailmanController(IBookRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var sve = _repository.GetForMailman().Where(s => s.ZaDostaviti || (s.Active && s.DanVracanja.Date.Equals(DateTime.Now.Date) && !s.ZaDostaviti)).ToList();
            return View(sve);
        }

        public IActionResult ZaDostaviti()
        {
            var dostaviti = _repository.GetForMailman().Where(p => p.ZaDostaviti).ToList();
            return View("Index", dostaviti);

        }

        public IActionResult ZaPreuzeti()
        {
            var preuzeti = _repository.GetForMailman().Where(s => s.Active && s.DanVracanja.Date.Equals(DateTime.Now.Date) && !s.ZaDostaviti).ToList();
            return View("Index", preuzeti);
        }

        public IActionResult Dostavi(Guid Id)
        {
            var posudba = _repository.GetPosudba(Id);
            posudba.ZaDostaviti = false;
            posudba.Active = true;
            _repository.UpdatePosudba(posudba);
            return RedirectToAction("Index");
        }

        public IActionResult Preuzmi(Guid Id)
        {
            var posudba = _repository.GetPosudba(Id);
            posudba.Active = false;
            var knjiga = _repository.Get(posudba.Book.BookId);
            knjiga.BorrowCounter++;
            _repository.Update(knjiga, posudba.UserId);
            _repository.UpdatePosudba(posudba);
            return RedirectToAction("Index");
        }

    }
}