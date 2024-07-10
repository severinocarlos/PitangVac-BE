using FluentValidation;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Messages;

namespace PitangVac.Validators.Fluent
{
    public class PatientRegisterValidator : AbstractValidator<PatientRegisterModel>
    {
        public PatientRegisterValidator() 
        {
            RuleFor(t => t.Name)
                .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Nome"))
                .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Nome"));

            RuleFor(t => t.Login)
               .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Login"))
               .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Login"))
               .MaximumLength(50).WithMessage(string.Format(BusinessMessages.MaxLength, "Login", 50));

            RuleFor(t => t.Email)
                .EmailAddress().WithMessage(string.Format(BusinessMessages.InvalidValue, "Email"))
                .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Email"))
                .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Email"));

            RuleFor(t => t.Password)
              .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Senha"))
              .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Senha"));

            RuleFor(t => t.BirthDate)
              .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Data de Nascimento"))
              .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Data de Nascimento"));
        }
    }
}
