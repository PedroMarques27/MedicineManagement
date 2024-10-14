using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public interface IPrescriptionRepository
    {
        ICollection<PrescriptionModel> GetAllPrescriptions();
        PrescriptionModel? GetPrescriptionById(Guid id);
        Task AddPrescriptionAsync(PrescriptionModel prescription);
        Task UpdatePrescriptionAsync(PrescriptionModel prescription);
        Task DeletePrescriptionByIdAsync(Guid id);
    }
}
