using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class MedicineModel
    {
        [Key]
        public required string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be a positive number.")]
        public required int Quantity { get; set; }
        public required DateTime CreationDate { get; set; }


    }
}
