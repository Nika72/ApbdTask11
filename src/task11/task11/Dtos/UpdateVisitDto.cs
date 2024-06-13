using System.ComponentModel.DataAnnotations;

namespace task11.Dtos
{
    public class UpdateVisitDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string Date { get; set; }
    }
}