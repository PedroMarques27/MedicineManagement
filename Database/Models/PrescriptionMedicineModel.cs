using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class PrescriptionMedicineModel
    {
        public required Guid PrescriptionId { get; set; }

        [ExcludeFromCodeCoverage]
        public virtual PrescriptionModel PrescriptionModel{ get; set; }

        public required string MedicineName { get; set; }

        [ExcludeFromCodeCoverage]
        public virtual MedicineModel Medicine { get; set; }
    }
}
