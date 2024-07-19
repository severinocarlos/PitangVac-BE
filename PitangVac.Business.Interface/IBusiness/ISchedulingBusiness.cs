using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;

namespace PitangVac.Business.Interface.IBusiness
{
    public interface ISchedulingBusiness
    {
        Task<SchedulingDTO> SchedulingRegister(SchedulingRegisterModel scheduling);
        Task<List<string>> HoursAvailable(DateTime date);
        Task<SchedulingDTO> SchedulingCompleted(int schedulingId);
        Task<SchedulingDTO> SchedulingCanceled(int schedulingId);
        Task<SchedulingPaginationDTO> GetAllSchedulingOrderedByDateAndTime(int pageNumber, int pageSize);
        Task<SchedulingPaginationDTO> GetSchedulingsByPatientIdOrderedByDateAndTime(int pageNumber, int pageSize);
        Task<SchedulingPaginationDTO> GetSchedulingsByStatusOrderedByDateAndTime(string status, int pageNumber, int pageSize);
    }
}
