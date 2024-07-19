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

        public async Task<SchedulingPaginationDTO> GetAllSchedulingOrderedByDateAndTime(int pageNumber, int pageSize)
        {
            return await _schedulingRepository.GetAllOrderedByDateAndTime(pageNumber, pageSize);
        }

        public async Task<SchedulingPaginationDTO> GetSchedulingsByPatientIdOrderedByDateAndTime(int pageNumber, int pageSize)
        {
            var login = _userContext.Login();
            // TODO: Refatorar para método que retorna apenas booleano verificando se existe por id
            var patient = await _patientRepository.FindByLogin(login) ?? 
                            throw new RegisterNotFound(string.Format(BusinessMessages.ValueNotFound, login));


            return await _schedulingRepository.GetByPatientIdOrderedByDateAndTime(patient.Id, pageNumber, pageSize);
        }

        public async Task<SchedulingPaginationDTO> GetSchedulingsByStatusOrderedByDateAndTime(string status, int pageNumber, int pageSize)
        {
            if (!StatusValidator.IsValidStatus(status))
            {
                throw new InvalidDataException(string.Format(BusinessMessages.InvalidValue, status));
            }

            return await _schedulingRepository.GetByStatusOrderedByDateAndTime(status, pageNumber, pageSize);
        }

        public async Task<List<string>> HoursAvailable(DateTime date)
        {
            // TODO: Validar data recebida

            var hours = await _schedulingRepository.FilledSchedules(date);

            // TODO: Melhorar a abordagem de selecionar as horas disponíveis
            var filteredTimeStrings = hours.Select(t => t.ToString(@"hh\:mm\:ss")).ToList();

           return HoursAvailableList.Where(e => !filteredTimeStrings.Contains(e)).ToList();
        }

        public async Task<SchedulingDTO> SchedulingCanceled(int schedulingId)
        {
            var scheduling = await _schedulingRepository.GetById(schedulingId) ?? throw new RegisterNotFound(string.Format(BusinessMessages.ValueNotFound, schedulingId));

            if (scheduling.Status != StatusEnum.Agendado)
            {
                throw new InvalidDataException(string.Format(BusinessMessages.InvalidValue, scheduling.Status));
            }

            scheduling.Status = StatusEnum.Cancelado;

            return new SchedulingDTO
            {
                Id = scheduling.Id,
                SchedulingDate = scheduling.SchedulingDate,
                SchedulingTime = scheduling.SchedulingTime,
                Status = scheduling.Status,
                CreateAt = scheduling.CreateAt,
                Patient = new PatientDTO
                {
                    Id = scheduling.Patient.Id,
                    Name = scheduling.Patient.Name,
                    Login = scheduling.Patient.Login,
                    Email = scheduling.Patient.Email,
                    BirthDate = scheduling.Patient.BirthDate,
                    CreateAt = scheduling.Patient.CreateAt,
                },
            };
        }

        public async Task<SchedulingDTO> SchedulingCompleted(int schedulingId)
        {
            var scheduling = await _schedulingRepository.GetById(schedulingId) ?? throw new RegisterNotFound(string.Format(BusinessMessages.ValueNotFound, schedulingId));

            if (scheduling.Status != StatusEnum.Agendado)
            {
                throw new InvalidDataException(string.Format(BusinessMessages.InvalidValue, scheduling.Status));
            }

            scheduling.Status = StatusEnum.Concluído;

            return new SchedulingDTO
            {
                Id = scheduling.Id,
                SchedulingDate = scheduling.SchedulingDate,
                SchedulingTime = scheduling.SchedulingTime,
                Status = scheduling.Status,
                CreateAt = scheduling.CreateAt,
                Patient = new PatientDTO
                {
                    Id = scheduling.Patient.Id,
                    Name = scheduling.Patient.Name,
                    Login = scheduling.Patient.Login,
                    Email = scheduling.Patient.Email,
                    BirthDate = scheduling.Patient.BirthDate,
                    CreateAt = scheduling.Patient.CreateAt,
                },
            };
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
