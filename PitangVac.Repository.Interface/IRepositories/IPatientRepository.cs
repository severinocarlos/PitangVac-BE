using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;

namespace PitangVac.Repository.Interface.IRepositories
{
    public interface IPatientRepository : IBaseRepository<Patient>
    {
        Task<bool> ExistByLogin(string login);
        Task<bool> ExistByEmail(string email);
        Task<List<PatientDTO>> FindByName(string name);
        Task<Patient?> FindByLogin(string login);
    }
}
