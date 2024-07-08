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

        public Task<SchedulingDTO?> FindByPatientId(int patientId)
        {
            var query = EntitySet.AsQueryable()
                                 .Select(scheduling => new SchedulingDTO
                                 {
                                    Id = scheduling.Id,
                                    PatientId = scheduling.PatientId,
                                    SchedulingDate = scheduling.SchedulingDate,
                                    SchedulingTime = scheduling.SchedulingTime,
                                    Status = scheduling.Status,
                                    CreateAt = scheduling.CreateAt
                                 });

            return query.FirstOrDefaultAsync(x => x.PatientId == patientId);
        }

        public Task<List<SchedulingDTO>> GetAllOrderedByDateAndTime()
        {
            var query = EntitySet.Select(scheduling => new SchedulingDTO
                                {
                                    Id = scheduling.Id,
                                    PatientId = scheduling.PatientId,
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
                                    PatientId = scheduling.PatientId,
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
            throw new NotImplementedException();
        }
    }
}
