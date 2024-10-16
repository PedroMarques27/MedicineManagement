using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public interface IMedicineRepository
    {
        ICollection<MedicineModel> GetAllMedicines();
        Task<MedicineModel?> GetMedicineByNameAsync(string name);
        Task AddMedicineAsync(MedicineModel medicine);
        Task UpdateMedicineAsync(MedicineModel medicine);
        Task DeleteMedicineByNameAsync(string Name);
        Task<bool> Exists(string Name);
    }
}
