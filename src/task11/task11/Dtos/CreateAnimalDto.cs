using System.ComponentModel.DataAnnotations;

namespace task11.Dtos
{
    public class CreateAnimalDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public int AnimalTypeId { get; set; }
    }
}