using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;

namespace PitangVac.Business.Interface.IBusiness
{
    public interface ISchedulingBusiness
    {
        Task<SchedulingDTO> SchedulingRegister(SchedulingRegisterModel scheduling);
        Task<SchedulingDTO> HoursAvailable();
        Task<SchedulingDTO> SchedulingCompleted(int schedulingId);
        Task<SchedulingDTO> SchedulingCanceled(int schedulingId);
        Task<List<SchedulingDTO>> GetAllSchedulingOrderedByDateAndTime();
        Task<List<SchedulingDTO>> GetSchedulingsByPatientIdOrderedByDateAndTime(int patientId);
        Task<List<SchedulingDTO>> GetSchedulingsByStatusOrderedByDateAndTime(string status);
    }
}
