using Assignment_14March.Data;
using Assignment_14March.Models;
using Assignment_14March.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_14March.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FullName", "Email", "Password", "ConfirmPassword", "Role", Prefix = "")] RegisterViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _context.Users.AnyAsync(u => u.Email == model.Email, cancellationToken))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email", "Password", Prefix = "")] LoginViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password, cancellationToken);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            if (user.Role == "Teacher")
                return RedirectToAction("Index", "TeacherDashboard");
            return RedirectToAction("Index", "StudentDashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}