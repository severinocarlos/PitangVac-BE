using Microsoft.EntityFrameworkCore;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;
using PitangVac.Repository.Interface.IRepositories;
using TaskControl.Repository.Repositories;

namespace PitangVac.Repository.Repositories
{
    public class SchedulingRepository : BaseRepository<Scheduling>, ISchedulingRepository
    {
        public SchedulingRepository(DatabaseContext dbContext) : base(dbContext) { }

        public Task<int> CheckSchedulingAvaliableByDate(DateTime date)
        {
            var query = EntitySet.AsQueryable();

            return query.CountAsync(scheduling => scheduling.SchedulingDate.Equals(date));
        }

        public Task<int> CheckSchedulingAvaliableByTime(TimeSpan hour)
        {
            var query = EntitySet.AsQueryable();

            return query.CountAsync(scheduling => scheduling.SchedulingTime.Equals(hour));
        }

        public Task<List<SchedulingDTO>> GetAllOrderedByDateAndTime()
        {
            var query = EntitySet.Select(scheduling => new SchedulingDTO
                                {
                                    Id = scheduling.Id,
                                    Patient = new PatientDTO
                                    {
                                        Id = scheduling.Patient.Id,
                                        Name = scheduling.Patient.Name,
                                        Login = scheduling.Patient.Login,
                                        Email = scheduling.Patient.Email,
                                        BirthDate = scheduling.Patient.BirthDate,
                                        CreateAt = scheduling.Patient.CreateAt,
                                    },
                                    SchedulingDate = scheduling.SchedulingDate,
                                    SchedulingTime = scheduling.SchedulingTime,
                                    Status = scheduling.Status,
                                    CreateAt = scheduling.CreateAt
                                })
                                .OrderBy(scheduling => scheduling.SchedulingDate)
                                .ThenBy(scheduling => scheduling.SchedulingTime);

            return query.ToListAsync();
        }

        public Task<List<SchedulingDTO>> GetByPatientIdOrderedByDateAndTime(int patientId)
        {
            var query = EntitySet.Where(scheduling => scheduling.PatientId == patientId)
                                 .Select(scheduling => new SchedulingDTO
                                 {
                                     Id = scheduling.Id,
                                     Patient = new PatientDTO
                                     {
                                         Id = scheduling.Patient.Id,
                                         Name = scheduling.Patient.Name,
                                         Login = scheduling.Patient.Login,
                                         Email = scheduling.Patient.Email,
                                         BirthDate = scheduling.Patient.BirthDate,
                                         CreateAt = scheduling.Patient.CreateAt,
                                     },
                                     SchedulingDate = scheduling.SchedulingDate,
                                     SchedulingTime = scheduling.SchedulingTime,
                                     Status = scheduling.Status,
                                     CreateAt = scheduling.CreateAt
                                 })
                                 .OrderBy(scheduling => scheduling.SchedulingDate)
                                 .ThenBy(scheduling => scheduling.SchedulingTime);

            return query.ToListAsync();
        }

        public Task<List<SchedulingDTO>> GetByStatusOrderedByDateAndTime(string status)
        {
            var query = EntitySet.Include(e => e.Patient)
                                 .Where(scheduling => scheduling.Status == status)
                                 .Select(scheduling => new SchedulingDTO
                                 {
                                      Id = scheduling.Id,
                                      Patient = new PatientDTO
                                      {
                                          Id = scheduling.Patient.Id,
                                          Name = scheduling.Patient.Name,
                                          Login = scheduling.Patient.Login,
                                          Email = scheduling.Patient.Email,
                                          BirthDate = scheduling.Patient.BirthDate,
                                          CreateAt = scheduling.Patient.CreateAt,
                                      },
                                      SchedulingDate = scheduling.SchedulingDate,
                                      SchedulingTime = scheduling.SchedulingTime,
                                      Status = scheduling.Status,
                                      CreateAt = scheduling.CreateAt
                                 })
                                 .OrderBy(scheduling => scheduling.SchedulingDate)
                                 .ThenBy(scheduling => scheduling.SchedulingTime);

            return query.ToListAsync();
        }
    }
}
