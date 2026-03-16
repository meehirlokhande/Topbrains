using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment_14March.Data;
using Assignment_14March.Models;

namespace Assignment_14March.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsTeacher() => HttpContext.Session.GetString("Role") == "Teacher";

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var list = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .OrderBy(s => s.StudentName)
                .ToListAsync(cancellationToken);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            await FillDropdownsAsync(cancellationToken);
            return View(new Student());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentName", "Email", "PhoneNumber", "Address", "DepartmentId", "CourseId", Prefix = "")] Student model, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.Students.Add(model);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Student added successfully.";
                return RedirectToAction(nameof(Index));
            }
            await FillDropdownsAsync(cancellationToken);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var student = await _context.Students.FindAsync(new object[] { id }, cancellationToken);
            if (student == null) return NotFound();
            await FillDropdownsAsync(cancellationToken);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId", "StudentName", "Email", "PhoneNumber", "Address", "DepartmentId", "CourseId", Prefix = "")] Student model, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            if (id != model.StudentId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Students.Update(model);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Student updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            await FillDropdownsAsync(cancellationToken);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.StudentId == id, cancellationToken);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var student = await _context.Students.FindAsync(new object[] { id }, cancellationToken);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Student deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task FillDropdownsAsync(CancellationToken cancellationToken)
        {
            ViewBag.Departments = new SelectList(await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(cancellationToken), "DepartmentId", "DepartmentName");
            ViewBag.Courses = new SelectList(await _context.Courses.OrderBy(c => c.CourseName).ToListAsync(cancellationToken), "CourseId", "CourseName");
        }
    }
}