using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment_14March.Data;
using Assignment_14March.Models;

namespace Assignment_14March.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsTeacher() => HttpContext.Session.GetString("Role") == "Teacher";

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var list = await _context.Courses.Include(c => c.Department).OrderBy(c => c.CourseName).ToListAsync(cancellationToken);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            ViewBag.Departments = GetDepartmentSelectList(await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(cancellationToken));
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseName", "Duration", "Fees", "DepartmentId", Prefix = "")] Course model, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.Courses.Add(model);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Course added successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = GetDepartmentSelectList(await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(cancellationToken));
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var course = await _context.Courses.FindAsync(new object[] { id }, cancellationToken);
            if (course == null) return NotFound();
            ViewBag.Departments = GetDepartmentSelectList(await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(cancellationToken));
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId", "CourseName", "Duration", "Fees", "DepartmentId", Prefix = "")] Course model, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            if (id != model.CourseId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Courses.Update(model);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Course updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = GetDepartmentSelectList(await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(cancellationToken));
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var course = await _context.Courses.Include(c => c.Department).FirstOrDefaultAsync(c => c.CourseId == id, cancellationToken);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var course = await _context.Courses.FindAsync(new object[] { id }, cancellationToken);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Course deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private static IEnumerable<SelectListItem> GetDepartmentSelectList(List<Department> departments)
        {
            var items = new List<SelectListItem> { new SelectListItem("-- Select Department --", "0") };
            foreach (var d in departments)
                items.Add(new SelectListItem(d.DepartmentName, d.DepartmentId.ToString()));
            return items;
        }
    }
}