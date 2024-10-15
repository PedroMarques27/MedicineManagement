using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Repositories.Models
{
    public class MockPrescriptionModel
    {
        public static PrescriptionModel Mock_Default()
        {
            return new PrescriptionModel
            {
                Id = Guid.Parse("28567025-a065-4c05-80d3-f0d09bd0b084"),
                CreationDate = DateTime.Now,
                UserEmail = "default@email",
                MedicineList = new List<MedicineModel> { MockMedicineModel.Mock_Default() }
            };
        }
        public static PrescriptionModel Mock()
        {
            return new PrescriptionModel
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                UserEmail = "useremail@email",
                MedicineList = new List<MedicineModel> { MockMedicineModel.Mock() }
            };
        }
    }
}
