using Process.DTOs;
using Process.DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Process.Entities;

namespace Tests.Process.DTOs
{
    public class MockUserInputDto
    {
        public static UserInputDto Mock_Default()
        {
            return new UserInputDto
            {
                Name = "default",
                Email = "default@email"
            };
        }
        public static UserInputDto Mock()
        {
            return new UserInputDto
            {
                Name = "username",
                Email = "useremail@email",
            };
        }
    }
}
