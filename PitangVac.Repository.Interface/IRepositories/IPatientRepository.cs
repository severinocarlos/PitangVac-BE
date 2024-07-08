using PitangVac.Entity.DTO;

namespace PitangVac.Repository.Interface.IRepositories
{
    public interface IPatientRepository
    {
        Task<bool> ExistByLogin(string login);
        Task<bool> ExistByEmail(string email);
        Task<PatientDTO> FindByName(string name);
    }
}
