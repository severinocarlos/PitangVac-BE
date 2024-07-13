using FluentValidation.TestHelper;
using NUnit.Framework;
using PitangVac.Business.Business;
using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.Models;
using PitangVac.Repository.Interface.IRepositories;
using PitangVac.Repository.Repositories;
using PitangVac.Utilities.Exceptions;
using PitangVac.Utilities.Messages;
using PitangVac.Validators.Fluent;

namespace PitangVac.UnitTest.InMemoryDB
{
    public class PatientBusinessTest : BaseUnitTest
    {
        private IPatientBusiness _business;
        private IPatientRepository _repository;
        private PatientRegisterValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _repository = new PatientRepository(_DbContext);

            _validator = new PatientRegisterValidator();


            RegistrarObjeto(typeof(IPatientRepository), _repository);

            Registrar<IPatientBusiness, PatientBusiness>();

            _business = ObterServico<IPatientBusiness>();
        }

        [Test]
        public void InsertPatient_Sucess()
        {
            var patient = new PatientRegisterModel 
            { 
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            result.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            async Task action() => await _business.SavePatient(patient);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task InsertPatient_AlreadyExist_ByLogin()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            result.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            await _business.SavePatient(patient);

            var newPatient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test1@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            async Task action() => await _business.SavePatient(newPatient);

            var excepetion = Assert.ThrowsAsync<ExistingResourceException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueAlreadyExist, patient.Login));
        }

        [Test]
        public async Task InsertPatient_AlreadyExist_ByEmail()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            result.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            result.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            await _business.SavePatient(patient);

            var newPatient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste1",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            async Task action() => await _business.SavePatient(newPatient);

            var excepetion = Assert.ThrowsAsync<ExistingResourceException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueAlreadyExist, patient.Email));
        }

        [TestCase("")]
        [TestCase(null)]
        public void InsertPatient_InvalidName(string name)
        {
            var patient = new PatientRegisterModel
            {
                Name = name,
                Login = "Teste",
                Email = "Test2@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldHaveValidationErrorFor(patient => patient.Name)
                  .WithErrorMessage(string.Format(BusinessMessages.RequiredValue, "Nome"));
        }

        [TestCase("")]
        [TestCase(null)]
        public void InsertPatient_InvalidLogin_WithNullOrEmptyValue(string login)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste 3",
                Login = login,
                Email = "Test3@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldHaveValidationErrorFor(patient => patient.Login)
                  .WithErrorMessage(string.Format(BusinessMessages.RequiredValue, "Login"));
        }

        [Test]
        public void InsertPatient_InvalidLogin_MaxLength_Greater_50()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste 3",
                Login = "qwertyuiopasdfghjklçzxcvbnmmnbvcxzlçkjhgfdsapoiuytrewqqwertyuiopasdfghjklçzxmcnvbvn",
                Email = "Test3@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldHaveValidationErrorFor(patient => patient.Login)
                  .WithErrorMessage(string.Format(BusinessMessages.MaxLength, "Login", 50));
        }

        [Test]
        public void InsertPatient_InvalidEmailAddress()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste 3",
                Login = "Teste",
                Email = "Test3teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldHaveValidationErrorFor(patient => patient.Email)
                  .WithErrorMessage(string.Format(BusinessMessages.InvalidValue, "Email"));
        }

        [TestCase("")]
        [TestCase(null)]
        public void InsertPatient_InvalidEmail_NullOrEmpty(string email)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste 3",
                Login = "Teste",
                Email = email,
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldHaveValidationErrorFor(patient => patient.Email)
                  .WithErrorMessage(string.Format(BusinessMessages.RequiredValue, "Email"));
        }

        [TestCase("")]
        [TestCase(null)]
        public void InsertPatient_InvalidPassword_NullOrEmpty(string password)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste 3",
                Login = "Teste",
                Email = "Teste@gmail.com",
                Password = password,
                BirthDate = DateTime.Now,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldHaveValidationErrorFor(patient => patient.Password)
                  .WithErrorMessage(string.Format(BusinessMessages.RequiredValue, "Senha"));
        }

        [TestCase(null)]
        public void InsertPatient_InvalidBirthDate_Null(DateTime birthDate)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste 3",
                Login = "Teste",
                Email = "Teste@gmail.com",
                Password = "Teste",
                BirthDate = birthDate,
            };

            var result = _validator.TestValidate(patient);
            result.ShouldHaveValidationErrorFor(patient => patient.BirthDate)
                  .WithErrorMessage(string.Format(BusinessMessages.InvalidValue, "Data de Nascimento"));
        }

        [Test]
        public void ChecksExistence_PatientByEmail_Success()
        {
            async Task action() => await _business.ExistPatientByEmail("teste@teste.com");

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ChecksExistence_PatientByEmail_Invalid(string email)
        {
            async Task action() => await _business.ExistPatientByEmail(email);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, email));
        }

        [Test]
        public void ChecksExistence_PatientByLogin_Success()
        {
            async Task action() => await _business.ExistPatientByLogin("teste");

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ChecksExistence_PatientByLogin_Invalid(string login)
        {
            async Task action() => await _business.ExistPatientByEmail(login);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, login));
        }

        [Test]
        public void FindPatientByName_WithSuccess()
        {
            async Task action() => await _business.FindPatientByName("teste");

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase("")]
        [TestCase(null)]
        public void FindPatientByName_Invalid(string name)
        {
            async Task action() => await _business.FindPatientByName(name);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, name));
        }

        [Test]
        public void FindPatientByLogin_WithSuccess()
        {
            async Task action() => await _business.FindPatientByLogin("teste");

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase("")]
        [TestCase(null)]
        public void FindPatientByLogin_Invalid(string login)
        {
            async Task action() => await _business.FindPatientByName(login);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, login));
        }
    }
}
