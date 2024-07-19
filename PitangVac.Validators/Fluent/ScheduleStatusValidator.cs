using FluentValidation;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Messages;

namespace PitangVac.Validators.Fluent
{
    public class ScheduleStatusValidator : AbstractValidator<HandleStatusModel>
    {
        public ScheduleStatusValidator() 
        {
            RuleFor(t => t.ScheduleId)
              .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "ScheduleId"))
              .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "ScheduleId"));

            RuleFor(t => t.PatientId)
              .NotNull().WithMessage(string.Format(BusinessMessages.RequiredValue, "PatientId"))
              .NotEmpty().WithMessage(string.Format(BusinessMessages.RequiredValue, "PatientId"));
        }
    }
}
