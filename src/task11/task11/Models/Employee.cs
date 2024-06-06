using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace task11.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }
    }
}