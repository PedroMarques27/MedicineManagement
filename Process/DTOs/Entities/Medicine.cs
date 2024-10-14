using Database.Models;
using System.ComponentModel.DataAnnotations;

namespace Process.DTOs.Entities
{
    public class Medicine
    {
        public string Name { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
