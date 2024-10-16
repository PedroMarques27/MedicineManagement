using Database.Models;
using Process.DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Repositories.Models;

namespace Tests.Process.Entities
{
    public class MockPrescription
    {
        public static Prescription Mock_Default()
        {
            return new Prescription
            {
                Id = Guid.Parse("28567025-a065-4c05-80d3-f0d09bd0b084"),
                CreationDate = DateTime.Now,
                MedicineList = new List<string> { MockMedicineModel.Mock().Name }
            };
        }
        public static Prescription Mock()
        {
            return new Prescription
            {
                Id = Guid.Parse("28567025-a065-4c05-80d3-f0d09bd0b085"),
                CreationDate = DateTime.Now,
                MedicineList = new List<string> { MockMedicineModel.Mock().Name }
            };
        }
    }
}
