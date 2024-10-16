using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Process.Entities
{
    public class MockPrescriptionMedicineModel
    {
        public static PrescriptionMedicineModel Mock_Default()
        {
            return new PrescriptionMedicineModel
            {
                PrescriptionId = Guid.Parse("28567025-a065-4c05-80d3-f0d09bd0b084"),
                MedicineName = "Default"
            };
        }
        public static PrescriptionMedicineModel Mock()
        {
            return new PrescriptionMedicineModel
            {
                PrescriptionId = Guid.Parse("28567025-a065-4c05-80d3-f0d09bd0b085"),
                MedicineName = "Paracetamol"
            };
        }
    }
}
