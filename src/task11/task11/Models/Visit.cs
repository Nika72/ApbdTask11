using System.ComponentModel.DataAnnotations;

namespace task11.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
        public DateTime Date { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }  // Concurrency token
    }
}