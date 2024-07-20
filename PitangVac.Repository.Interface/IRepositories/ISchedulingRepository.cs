using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;

namespace PitangVac.Repository.Interface.IRepositories
{
    public interface ISchedulingRepository : IBaseRepository<Scheduling>
    {
        Task<SchedulingPaginationDTO> GetAllOrderedByDateAndTime(int pageNumber, int pageSize);
        Task<SchedulingPaginationDTO> GetByPatientIdOrderedByDateAndTime(int patientId, int pageNumber, int pageSize);
        Task<SchedulingPaginationDTO> GetByStatusOrderedByDateAndTime(string status, int patientId, int pageNumber, int pageSize);
        Task<int> CheckSchedulingAvaliableByDate(DateTime date);
        Task<int> CheckSchedulingAvaliableByTime(DateTime date, TimeSpan hour);
        Task<List<TimeSpan>> FilledSchedules(DateTime date);
    }
}
