using Microsoft.AspNetCore.Mvc;

namespace Assignment_14March.Controllers
{
    public class TeacherDashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Teacher")
                return RedirectToAction("Login", "Account");
            return View();
        }
    }
}