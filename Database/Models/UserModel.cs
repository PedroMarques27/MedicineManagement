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
        public string Name { get; set; }

        [Key]
        public string Email { get; set; }

        public List<PrescriptionModel> PrescriptionList { get; set; }
    }
}
