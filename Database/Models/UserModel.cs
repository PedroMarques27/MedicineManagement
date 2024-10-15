using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class UserModel
    {
        public required string Name { get; set; }

        [Key]
        public required string Email { get; set; }

        public required List<PrescriptionModel> PrescriptionList { get; set; }
    }
}
