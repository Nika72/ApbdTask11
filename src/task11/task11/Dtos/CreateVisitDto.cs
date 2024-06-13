using System.ComponentModel.DataAnnotations;

namespace task11.Dtos
{
    public class CreateVisitDto
    {
        [Required]
        public int AnimalId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string Date { get; set; }
    }
}