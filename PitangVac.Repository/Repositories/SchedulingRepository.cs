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

        public Task<int> CheckSchedulingAvaliableByTime(DateTime date, TimeSpan hour)
        {
            var query = EntitySet.AsQueryable();

            return query.Where(e => e.SchedulingDate.Equals(date))
                        .CountAsync(scheduling => scheduling.SchedulingTime.Equals(hour));
        }

        public async Task<SchedulingPaginationDTO> GetAllOrderedByDateAndTime(int pageNumber, int pageSize)
        {
            // TODO: Verificar se é a melhor forma de fazer a paginação

            var totalCount = await EntitySet.CountAsync();

            var schedulings = await EntitySet.Select(scheduling => new SchedulingDTO
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
                                .Skip(pageNumber * pageSize)
                                .Take(pageSize)
                                .OrderBy(scheduling => scheduling.SchedulingDate)
                                .ThenBy(scheduling => scheduling.SchedulingTime)
                                .ToListAsync();

            return new SchedulingPaginationDTO
            {
                Schedulings = schedulings,
                TotalLength = totalCount
            };
        }

        public async Task<SchedulingPaginationDTO> GetByPatientIdOrderedByDateAndTime(int patientId, int pageNumber, int pageSize)
        {
            var totalCount = await EntitySet.CountAsync();

            var schedulings = await EntitySet.Where(scheduling => scheduling.PatientId == patientId)
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
                                 .Skip(pageNumber * pageSize)
                                 .Take(pageSize)
                                 .OrderBy(scheduling => scheduling.SchedulingDate)
                                 .ThenBy(scheduling => scheduling.SchedulingTime)
                                 .ToListAsync();

            return new SchedulingPaginationDTO
            {
                Schedulings = schedulings,
                TotalLength = totalCount
            };
        }

        public async Task<SchedulingPaginationDTO> GetByStatusOrderedByDateAndTime(string status, int pageNumber, int pageSize)
        {

            var totalCount = await EntitySet.CountAsync();

            var schedulings = await EntitySet.Include(e => e.Patient)
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
                                 .Skip(pageNumber * pageSize)
                                 .Take(pageSize)
                                 .OrderBy(scheduling => scheduling.SchedulingDate)
                                 .ThenBy(scheduling => scheduling.SchedulingTime)
                                 .ToListAsync();

            return new SchedulingPaginationDTO
            {
                Schedulings = schedulings,
                TotalLength = totalCount
            };
        }

        public Task<List<TimeSpan>> FilledSchedules(DateTime date)
        {
            var query = EntitySet.Where(e => e.SchedulingDate.Equals(date))
                                        .GroupBy(e => e.SchedulingTime)
                                        .Where(g => g.Count() == 2)
                                        .Select(g => g.Key);

            return query.ToListAsync();
        }
    }
}
