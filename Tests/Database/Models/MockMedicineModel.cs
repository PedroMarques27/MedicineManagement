using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;

namespace Tests.Repositories.Models
{
    public static class MockMedicineModel
    {
        public static MedicineModel Mock_Default()
        {
            return new MedicineModel
            {
                Name = "Default",
                Quantity = 10,
                CreationDate = DateTime.Now,
            };
        }
        public static MedicineModel Mock()
        {
            return new MedicineModel
            {
                Name = "Paracetamol",
                Quantity = 10,
                CreationDate = DateTime.Now,
            };
        }
        
    }
}
