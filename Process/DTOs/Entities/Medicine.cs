using Database.Models;
using System.ComponentModel.DataAnnotations;

namespace Process.DTOs.Entities
{
    public class Medicine
    {
        public required string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be greater than or equal to 0.")]
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
