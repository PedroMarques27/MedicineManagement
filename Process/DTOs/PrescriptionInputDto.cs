namespace Process.DTOs
{
    public class PrescriptionInputDto
    {
        public ICollection<string> MedicineList { get; set; } = new List<string>();
    }
}
