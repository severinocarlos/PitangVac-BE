using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;

namespace PitangVac.Repository.Interface.IRepositories
{
    public interface ISchedulingRepository : IBaseRepository<Scheduling>
    {
        Task<List<SchedulingDTO>> GetAllOrderedByDateAndTime();
        Task<SchedulingDTO?> FindByPatientId(int patientId);
        Task<List<SchedulingDTO>> GetByPatientIdOrderedByDateAndTime(int patientId);
        Task<List<SchedulingDTO>> GetByStatusOrderedByDateAndTime(string status);
    }
}
