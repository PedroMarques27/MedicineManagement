using FluentValidation;
using Process.DTOs.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Process.Validators
{
    [ExcludeFromCodeCoverage]
    public class MedicineValidator: AbstractValidator<Medicine>
    {
        public MedicineValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Value must be greater than or equal to 0.");
        }
    }
}
