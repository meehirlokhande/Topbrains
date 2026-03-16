using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment_14March.Data;
using Assignment_14March.Models;
using Assignment_14March.ViewModels;

namespace Assignment_14March.Controllers
{
    public class StudentDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsStudent() => HttpContext.Session.GetString("Role") == "Student";

        private async Task<Student?> GetCurrentStudentAsync(CancellationToken cancellationToken)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userId == null || string.IsNullOrEmpty(userEmail)) return null;
            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);
            if (student != null) return student;
            student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Email == userEmail, cancellationToken);
            if (student != null)
            {
                student.UserId = userId;
                await _context.SaveChangesAsync(cancellationToken);
            }
            return student;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            if (!IsStudent()) return RedirectToAction("Login", "Account");
            var student = await GetCurrentStudentAsync(cancellationToken);
            if (student == null)
            {
                TempData["Message"] = "No student record linked to your account. Contact admin.";
                return View();
            }
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            if (!IsStudent()) return RedirectToAction("Login", "Account");
            var student = await GetCurrentStudentAsync(cancellationToken);
            if (student == null) return RedirectToAction("Index");
            var vm = new StudentProfileViewModel
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                Address = student.Address,
                DepartmentName = student.Department?.DepartmentName ?? "",
                CourseName = student.Course?.CourseName ?? ""
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile(CancellationToken cancellationToken)
        {
            if (!IsStudent()) return RedirectToAction("Login", "Account");
            var student = await GetCurrentStudentAsync(cancellationToken);
            if (student == null) return RedirectToAction("Index");
            var vm = new StudentUpdateProfileViewModel
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Email = student.Email,
                DepartmentName = student.Department?.DepartmentName ?? "",
                CourseName = student.Course?.CourseName ?? "",
                PhoneNumber = student.PhoneNumber,
                Address = student.Address
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([Bind("StudentId", "PhoneNumber", "Address", Prefix = "")] StudentUpdateProfileViewModel model, CancellationToken cancellationToken)
        {
            if (!IsStudent()) return RedirectToAction("Login", "Account");
            var student = await _context.Students.FindAsync(new object[] { model.StudentId }, cancellationToken);
            if (student == null) return RedirectToAction("Index");
            student.PhoneNumber = model.PhoneNumber;
            student.Address = model.Address;
            await _context.SaveChangesAsync(cancellationToken);
            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public async Task<IActionResult> Course(CancellationToken cancellationToken)
        {
            if (!IsStudent()) return RedirectToAction("Login", "Account");
            var student = await GetCurrentStudentAsync(cancellationToken);
            if (student == null) return RedirectToAction("Index");
            if (student.Course == null)
            {
                TempData["Message"] = "No course assigned.";
                return RedirectToAction("Index");
            }
            return View(student.Course);
        }
    }
}