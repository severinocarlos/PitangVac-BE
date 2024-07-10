using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;
using PitangVac.Entity.Models;
using PitangVac.Repository.Interface.IRepositories;
using PitangVac.Utilities.Exceptions;
using PitangVac.Utilities.Messages;
using System.Security.Cryptography;
using System.Text;

namespace PitangVac.Business.Business
{
    public class PatientBusiness : IPatientBusiness
    {

        private readonly IPatientRepository _patientRepository;

        public PatientBusiness(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientDTO> SavePatient(PatientRegisterModel newPatient)
        {
            if (await ExistPatientByEmail(newPatient.Email))
            {
                throw new ExistingResourceException(string.Format(BusinessMessages.ValueAlreadyExist, newPatient.Email));
            }

            if (await ExistPatientByLogin(newPatient.Login))
            {
                throw new ExistingResourceException(string.Format(BusinessMessages.ValueAlreadyExist, newPatient.Login));
            }

            using var hmac = new HMACSHA512();

            var patient = new Patient
            {
                Name = newPatient.Name,
                Login = newPatient.Login,
                Email = newPatient.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPatient.Password)),
                PasswordSalt = hmac.Key,
                BirthDate = newPatient.BirthDate,
                CreateAt = DateTime.Now
            };

            var patientCreated = await _patientRepository.Save(patient);

            return new PatientDTO
            {
                Id = patientCreated.Id,
                Name = patientCreated.Name,
                Login = patientCreated.Login,
                Email = patientCreated.Email,
                BirthDate = patientCreated.BirthDate,
                CreateAt = patientCreated.CreateAt
            };
        }


        public async Task<bool> ExistPatientByEmail(string email)
        {
            return await _patientRepository.ExistByEmail(email);
        }

        public async Task<bool> ExistPatientByLogin(string login)
        {
            return await _patientRepository.ExistByLogin(login);
        }

        public async Task<List<PatientDTO>> FindPatientByName(string name)
        {
            return await _patientRepository.FindByName(name);
        }

        public async Task<Patient?> FindPatientByLogin(string login)
        {
            return await _patientRepository.FindByLogin(login);
        }
    }
}
