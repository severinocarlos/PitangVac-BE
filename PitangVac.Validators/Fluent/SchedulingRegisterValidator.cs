using FluentValidation;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Messages;
using PitangVac.Validators.Manual;

namespace PitangVac.Validators.Fluent
{
    public class SchedulingRegisterValidator : AbstractValidator<SchedulingRegisterModel>
    {
        public SchedulingRegisterValidator() 
        {
            RuleFor(t => t.PatientId)
               .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Id do paciente"))
               .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Id do paciente"));

            RuleFor(t => t.SchedulingDate)
                .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Data do agendamento"))
                .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Data do agendamento"));

            RuleFor(t => t.SchedulingTime)
                .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "Hora do agendamento"))
                .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "Hora do agendamento"));
        }
    }
}
