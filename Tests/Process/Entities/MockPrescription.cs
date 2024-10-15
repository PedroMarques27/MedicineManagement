using Database.Models;
using Process.DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                MedicineList = new List<Medicine> { MockMedicine.Mock_Default() }
            };
        }
        public static Prescription Mock()
        {
            return new Prescription
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                MedicineList = new List<Medicine> { MockMedicine.Mock() }
            };
        }
    }
}
