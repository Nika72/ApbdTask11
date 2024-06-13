using System;
using System.ComponentModel.DataAnnotations;

namespace task11.Models
{
    public class Visit
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int AnimalId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Employee Employee { get; set; }
        public Animal Animal { get; set; }
    }
}