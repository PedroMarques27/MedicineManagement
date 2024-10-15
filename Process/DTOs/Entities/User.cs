using Database.Models;
using System.ComponentModel.DataAnnotations;

namespace Process.DTOs.Entities
{
    public class User
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public List<Prescription> PrescriptionList { get; set; } = new List<Prescription>();
    }
}
