using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment_14March.Data;
using Assignment_14March.Models;

namespace Assignment_14March.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsTeacher() => HttpContext.Session.GetString("Role") == "Teacher";

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var list = await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(cancellationToken);
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            return View(new Department());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentName", "Description", Prefix = "")] Department model, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.Departments.Add(model);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Department added successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var dept = await _context.Departments.FindAsync(new object[] { id }, cancellationToken);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId", "DepartmentName", "Description", Prefix = "")] Department model, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            if (id != model.DepartmentId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Departments.Update(model);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Department updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var dept = await _context.Departments.FindAsync(new object[] { id }, cancellationToken);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            if (!IsTeacher()) return RedirectToAction("Login", "Account");
            var dept = await _context.Departments.FindAsync(new object[] { id }, cancellationToken);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["Success"] = "Department deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}