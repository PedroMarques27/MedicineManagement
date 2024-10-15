using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Repositories.Models
{
    public static class MockUserModel
    {
        public static UserModel Mock_Default()
        {
            return new UserModel
            {
                Name = "default",
                Email = "default@email",
                PrescriptionList = new List<PrescriptionModel> { MockPrescriptionModel.Mock_Default() }
            };
        }
        public static UserModel Mock()
        {
            return new UserModel
            {
                Name = "username",
                Email = "useremail@email",
                PrescriptionList = new List<PrescriptionModel> { MockPrescriptionModel.Mock() }
            };
        }
    }
}
