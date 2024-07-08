using Microsoft.EntityFrameworkCore;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;
using PitangVac.Repository.Interface.IRepositories;
using System.Xml.XPath;
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

        public Task<List<PatientDTO>> FindByName(string name)
        {
            var query = EntitySet.Where(x => x.Name == name)
                                 .Select(patient => new PatientDTO
                                 {
                                     Id = patient.Id,
                                     Name = patient.Name,
                                     Login = patient.Login,
                                     Email = patient.Email,
                                     BirthDate = patient.BirthDate,
                                     CreateAt = patient.CreateAt
                                 })
                                 .OrderBy(patient => patient.Name);


            return query.ToListAsync();
        }
    }
}
