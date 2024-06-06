using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace task11.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AnimalTypesId { get; set; }
        public AnimalType AnimalType { get; set; }
        public ICollection<Visit> Visits { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }  // Concurrency token
    }
}