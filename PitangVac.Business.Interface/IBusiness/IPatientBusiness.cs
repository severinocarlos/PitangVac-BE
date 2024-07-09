using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;

namespace PitangVac.Business.Interface.IBusiness
{
    public interface IPatientBusiness
    {
        Task<PatientDTO> SavePatient(PatientModel patientDTO);
        Task<bool> ExistPatientByLogin(string login);
        Task<bool> ExistPatientByEmail(string email);
        Task<List<PatientDTO>> FindPatientByName(string name);
    }
}
