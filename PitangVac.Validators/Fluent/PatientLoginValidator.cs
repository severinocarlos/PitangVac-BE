using FluentValidation;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Messages;

namespace PitangVac.Validators.Fluent
{
    public class PatientLoginValidator : AbstractValidator<LoginModel>
    {
        public PatientLoginValidator()
        {
            
            RuleFor(t => t.Login)
               .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Login"))
               .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Login"))
               .MaximumLength(50).WithMessage(string.Format(BusinessMessages.MaxLength, "Login", 50));

            RuleFor(t => t.Password)
              .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Senha"))
              .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Senha"));

        }
    }
}
