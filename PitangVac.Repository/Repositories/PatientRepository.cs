using Microsoft.EntityFrameworkCore;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;
using PitangVac.Repository.Interface.IRepositories;
using TaskControl.Repository.Repositories;

namespace PitangVac.Repository.Repositories
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(DatabaseContext context) : base(context) { }

        public Task<bool> ExistByEmail(string email)
        {
            var query = EntitySet.AsQueryable();

            return query.AnyAsync(x => x.Email == email);
        }

        public Task<bool> ExistByLogin(string login)
        {
            var query = EntitySet.AsQueryable();

            return query.AnyAsync(x => x.Login == login);
        }

        public Task<PatientDTO> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
