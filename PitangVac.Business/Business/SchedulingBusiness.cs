using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;
using PitangVac.Entity.Enums;
using PitangVac.Entity.Models;
using PitangVac.Repository.Interface.IRepositories;
using PitangVac.Utilities.Exceptions;
using PitangVac.Utilities.Extensions;
using PitangVac.Utilities.Messages;
using PitangVac.Utilities.UserContext;
using PitangVac.Validators.Manual;

namespace PitangVac.Business.Business
{
    public class SchedulingBusiness : ISchedulingBusiness
    {
        private static List<string> HoursAvailableList = new()
        {
            "08:00:00",
            "09:00:00",
            "10:00:00",
            "11:00:00",
            "12:00:00",
            "13:00:00",
            "14:00:00",
            "15:00:00",
            "16:00:00",
            "17:00:00"
        };

        private readonly ISchedulingRepository _schedulingRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserContext _userContext;

        public SchedulingBusiness(ISchedulingRepository schedulingRepository, 
                                  IPatientRepository patientRepository,
                                  IUserContext userContext) 
        {
            _schedulingRepository = schedulingRepository;
            _patientRepository = patientRepository;
            _userContext = userContext;
        }

        public async Task<List<SchedulingDTO>> GetAllSchedulingOrderedByDateAndTime()
        {
            return await _schedulingRepository.GetAllOrderedByDateAndTime();
        }

        public async Task<List<SchedulingDTO>> GetSchedulingsByPatientIdOrderedByDateAndTime(int patientId)
        {
            // TODO: Refatorar para método que retorna apenas booleano verificando se existe por id
            var _ = await _patientRepository.GetById(patientId) ?? 
                            throw new RegisterNotFound(string.Format(BusinessMessages.ValueNotFound, patientId));


            return await _schedulingRepository.GetByPatientIdOrderedByDateAndTime(patientId);
        }

        public async Task<List<SchedulingDTO>> GetSchedulingsByStatusOrderedByDateAndTime(string status)
        {
            if (!StatusValidator.IsValidStatus(status))
            {
                throw new InvalidDataException(string.Format(BusinessMessages.InvalidValue, status));
            }

            return await _schedulingRepository.GetByStatusOrderedByDateAndTime(status);
        }

        public async Task<List<string>> HoursAvailable(DateTime date)
        {
            // TODO: Validar data recebida

            var hours = await _schedulingRepository.FilledSchedules(date);

            // TODO: Melhorar a abordagem de selecionar as horas disponíveis
            var filteredTimeStrings = hours.Select(t => t.ToString(@"hh\:mm\:ss")).ToList();

           return HoursAvailableList.Where(e => !filteredTimeStrings.Contains(e)).ToList();
        }

        public async Task<List<SchedulingDTO>> SchedulingCanceled(int schedulingId)
        {
            var scheduling = await _schedulingRepository.GetById(schedulingId) ?? throw new RegisterNotFound(string.Format(BusinessMessages.ValueNotFound, schedulingId));

            if (scheduling.Status != StatusEnum.Agendado)
            {
                throw new InvalidDataException(string.Format(BusinessMessages.InvalidValue, scheduling.Status));
            }

            scheduling.Status = StatusEnum.Cancelado;

            return await _schedulingRepository.GetAllOrderedByDateAndTime();
        }

        public async Task<List<SchedulingDTO>> SchedulingCompleted(int schedulingId)
        {
            var scheduling = await _schedulingRepository.GetById(schedulingId) ?? throw new RegisterNotFound(string.Format(BusinessMessages.ValueNotFound, schedulingId));

            if (scheduling.Status != StatusEnum.Agendado)
            {
                throw new InvalidDataException(string.Format(BusinessMessages.InvalidValue, scheduling.Status));
            }

            scheduling.Status = StatusEnum.Concluído;

            return await _schedulingRepository.GetAllOrderedByDateAndTime();
        }

        public async Task<SchedulingDTO> SchedulingRegister(SchedulingRegisterModel scheduling)
        {
            var login = _userContext.Login();

            var patient = await _patientRepository.FindByLogin(login);

            var maximumSchedulingAmountPerDay = 20;
            var schedulingAmountPerDay = await _schedulingRepository.CheckSchedulingAvaliableByDate(scheduling.SchedulingDate);

            if (schedulingAmountPerDay == maximumSchedulingAmountPerDay)
            {
                throw new BusinessException(string.Format(BusinessMessages.MaximumSchedulingQuantity, "dia", scheduling.SchedulingDate));
            }

            var maximumSchedulingAmountPerTime = 2;
            var schedulingAmountPerTime = await _schedulingRepository.CheckSchedulingAvaliableByTime(scheduling.SchedulingDate, scheduling.SchedulingTime);

            if (schedulingAmountPerTime == maximumSchedulingAmountPerTime)
            {
                throw new BusinessException(string.Format(BusinessMessages.MaximumSchedulingQuantity, "hora", scheduling.SchedulingTime));
            }

            var newScheduling = new Scheduling
            {
                PatientId = patient!.Id,
                SchedulingDate = scheduling.SchedulingDate,
                SchedulingTime = scheduling.SchedulingTime,
                Status = StatusEnum.Agendado,
                CreateAt = DateTime.Now,
            };


            var scheduleCreated = await _schedulingRepository.Save(newScheduling);

            return new SchedulingDTO
            {
                Id = scheduleCreated.Id,
                Patient = new PatientDTO
                {
                    Id = scheduleCreated.Patient.Id,
                    Name = scheduleCreated.Patient.Name,
                    Login = scheduleCreated.Patient.Login,
                    Email = scheduleCreated.Patient.Email,
                    BirthDate = scheduleCreated.Patient.BirthDate,
                    CreateAt = scheduleCreated.Patient.CreateAt,
                },
                SchedulingDate = scheduleCreated.SchedulingDate,
                SchedulingTime = scheduleCreated.SchedulingTime,
                Status = scheduleCreated.Status,
                CreateAt = scheduleCreated.CreateAt,
            };
        }
    }
}
