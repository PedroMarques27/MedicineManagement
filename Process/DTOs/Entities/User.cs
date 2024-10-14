using Database.Models;
using System.ComponentModel.DataAnnotations;

namespace Process.DTOs.Entities
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Prescription> PrescriptionList { get; set; } = new List<Prescription>();
    }
}
