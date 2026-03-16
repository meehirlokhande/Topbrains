using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_14March.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}