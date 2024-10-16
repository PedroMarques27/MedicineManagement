using Database.Models;

namespace Process.DTOs.Entities
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public List<string> MedicineList { get; set; } = new List<string>();
    }
}
