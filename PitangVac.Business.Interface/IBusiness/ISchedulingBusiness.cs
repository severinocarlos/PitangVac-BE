using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;

namespace PitangVac.Business.Interface.IBusiness
{
    public interface ISchedulingBusiness
    {
        Task<SchedulingDTO> SchedulingRegister(SchedulingRegisterModel scheduling);
        Task<List<string>> HoursAvailable(DateTime date);
        Task<SchedulingDTO> SchedulingCompleted(HandleStatusModel statusModel);
        Task<SchedulingDTO> SchedulingCanceled(HandleStatusModel statusModel);
        Task<SchedulingPaginationDTO> GetAllSchedulingOrderedByDateAndTime(int pageNumber, int pageSize);
        Task<SchedulingPaginationDTO> GetSchedulingsByPatientIdOrderedByDateAndTime(int patientId, int pageNumber, int pageSize);
        Task<SchedulingPaginationDTO> GetSchedulingsByStatusOrderedByDateAndTime(string status, int pageNumber, int pageSize);
    }
}
