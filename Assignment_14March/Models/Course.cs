using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_14March.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Duration { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Fees { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a department.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; } 

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}