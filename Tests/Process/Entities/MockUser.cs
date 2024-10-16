using Database.Models;
using Process.DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Process.Entities
{
    public static class MockUser
    {
        public static User Mock_Default()
        {
            return new User
            {
                Name = "default",
                Email = "default@email",
                PrescriptionList = new List<Prescription> { MockPrescription.Mock_Default() }
            };
        }
        public static User Mock()
        {
            return new User
            {
                Name = "username",
                Email = "useremail@email",
                PrescriptionList = new List<Prescription> { MockPrescription.Mock() }
            };
        }
    }
}
