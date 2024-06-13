using System.ComponentModel.DataAnnotations;

namespace task11.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string RefreshToken { get; set; }

        [Required]
        public string Role { get; set; } = "User";
    }
}