using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace task11.Models
{
    public class Animal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public int AnimalTypesId { get; set; }

        public AnimalType AnimalType { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }  // Concurrency token

        public ICollection<Visit> Visits { get; set; }
    }
}