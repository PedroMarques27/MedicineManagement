using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using Process.DTOs.Entities;

namespace Tests.Process.Entities
{
    public static class MockMedicine
    {
        public static Medicine Mock_Default()
        {
            return new Medicine
            {
                Name = "Default",
                Quantity = 10,
                CreationDate = DateTime.Now,
            };
        }
        public static Medicine Mock()
        {
            return new Medicine
            {
                Name = "Paracetamol",
                Quantity = 10,
                CreationDate = DateTime.Now,
            };
        }
        public static Medicine Mock_InvalidQuantity()
        {
            return new Medicine
            {
                Name = "BenURon",
                Quantity = -100,
                CreationDate = DateTime.Now,
            };
        }

    }
}
