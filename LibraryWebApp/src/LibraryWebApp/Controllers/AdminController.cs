using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LibraryWebApp.Interfaces;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace LibraryWebApp.Controllers
{
    [Authorize(Policy="Administrators")]
    public class AdminController : Controller
    {

        private IBookRepository _repository;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IHostingEnvironment _hostingEnvironment;

        public AdminController(IBookRepository repository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment hostingEnvionment)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvionment;

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
            BookViewModel m = new BookViewModel();
            m.Books = _repository.GetAllBooks();
            return View("Adder",m);

        }

        public async Task<IActionResult> Users()
        {
            var AllUsers = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Member"));
            var Users = new List<ApplicationUser>();
            foreach(var user in AllUsers)
            {
                var standardClaims = await _userManager.GetClaimsAsync(user);
                if (standardClaims.Any(p => p.Value.Equals("Admin")) || standardClaims.Any(p => p.Value.Equals("Mailman")))
                {
                    continue;
                }
                Users.Add(user);
            }
            return View(Users);   
        }

        public async Task<IActionResult> Admins()
        {
            var Users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Admin"));

            return View(Users.ToList());
        }
        public async Task<IActionResult> Mailmans()
        {
            var Users = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Mailman"));

            return View(Users.ToList());
        }

        public async Task<IActionResult> MakeMailman(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Mailman"));

            return RedirectToAction("Users");
        }


        public async Task<IActionResult> MakeAdmin(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
            return RedirectToAction("Users");

        }

        public async Task<IActionResult> RemoveMailman(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, "Mailman"));

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> RemoveAdmin(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            var standardClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
            var stdClaims = await _userManager.GetClaimsAsync(user);

            return RedirectToAction("Users");
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

                if(m.image != null)
                {

                    var webRoot = _hostingEnvironment.WebRootPath;
                    var file = System.IO.Path.Combine(webRoot + "/images/bookimages", m.image.FileName);
                    using (FileStream fs = new FileStream(file, FileMode.Create))
                    {
                        await m.image.CopyToAsync(fs);
                    }
                }
                string path;
                if (m.image == null)
                {
                    path = null;
                }
                else
                {
                    path = m.image.FileName;
                }

                if (knjiga != null)
                {

                    Writer pisac1 = knjiga.Writer;
                      //public Book(string title, Writer writer, Guid userId, string about, Genres genre,
                        // int salecounter, int borrowcounter, bool zakupnju, bool zaposudbu, double price)
                    item = new Book(m.Text, pisac1, Guid.Parse(currentUser.Id), m.About, m.Genre,
                        m.SaleCounter, m.BorrowCounter, m.ZaKupnju, m.ZaPosudbu, m.Price, path);
                }
                else
                {
                    item = new Book(m.Text, pisac, Guid.Parse(currentUser.Id), m.About, m.Genre,
                         m.SaleCounter, m.BorrowCounter, m.ZaKupnju, m.ZaPosudbu, m.Price, path);
                }

                var test = _repository.GetAllBooks().FirstOrDefault(a => (a.Title.Equals(item.Title) &&
                a.Writer.FirstName.Equals(item.Writer.FirstName) && a.Writer.LastName.Equals(item.Writer.LastName)));

                if (test == null)
                {
                    _repository.Add(item);
                }
                else
                {
                    test.BorrowCounter += item.BorrowCounter;
                    test.SaleCounter += item.SaleCounter;
                    _repository.Update(test, test.UserId);
                }

                return RedirectToAction("Add");
            }

            return View("Adder", m);
        }


        public async Task<IActionResult> DeleteBook(Guid Id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var book = _repository.Get(Id);
            if (book.ImgPath != null)
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var file = System.IO.Path.Combine(webRoot + "/images/bookimages", book.ImgPath);
                System.IO.File.Delete(file);
            }
            _repository.Remove(Id, Guid.Parse(currentUser.Id));
            return RedirectToAction("Add");

        }

        

    }
}
