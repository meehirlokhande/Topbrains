namespace Assignment_14March.ViewModels
{
    public class StudentProfileViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
    }
}