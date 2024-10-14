using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
namespace Database.Models
{
    public class PrescriptionModel
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }


        public string UserEmail { get; set; }
        public UserModel User { get; set; }


        public ICollection<MedicineModel> MedicineList { get; set; } = new List<MedicineModel>();
    }
}
