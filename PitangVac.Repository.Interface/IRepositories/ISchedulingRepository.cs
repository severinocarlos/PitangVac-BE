using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;

namespace PitangVac.Repository.Interface.IRepositories
{
    public interface ISchedulingRepository : IBaseRepository<Scheduling>
    {
        Task<List<SchedulingDTO>> GetAllOrderedByDateAndTime();
        Task<List<SchedulingDTO>> GetByPatientIdOrderedByDateAndTime(int patientId);
        Task<List<SchedulingDTO>> GetByStatusOrderedByDateAndTime(string status);
        Task<int> CheckSchedulingAvaliableByDate(DateTime date);
        Task<int> CheckSchedulingAvaliableByTime(DateTime date, TimeSpan hour);
        Task<List<TimeSpan>> FilledSchedules(DateTime date);
    }
}
