using System.ComponentModel.DataAnnotations;

namespace Assignment_14March.Models
{
    public class User
    {
       
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100,MinimumLength=6)]
        public string Password { get; set; }
        [Required]
        [StringLength (100,MinimumLength=20)]
        public string Role { get; set; }

    }
}
