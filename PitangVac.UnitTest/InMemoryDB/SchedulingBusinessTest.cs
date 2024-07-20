using FluentValidation.TestHelper;
using NUnit.Framework;
using PitangVac.Business.Business;
using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.Entities;
using PitangVac.Entity.Enums;
using PitangVac.Entity.Models;
using PitangVac.Repository.Interface.IRepositories;
using PitangVac.Repository.Repositories;
using PitangVac.Utilities.Exceptions;
using PitangVac.Utilities.Messages;
using PitangVac.Validators.Fluent;

namespace PitangVac.UnitTest.InMemoryDB
{
    public class SchedulingBusinessTest : BaseUnitTest
    {
        private ISchedulingBusiness _schedulingBusiness;
        private IPatientBusiness _patientBusiness;

        private ISchedulingRepository _schedulingRepository;
        private IPatientRepository _patientRepository;

        private SchedulingRegisterValidator _schedulingValidator;
        private PatientRegisterValidator _patientValidator;
        
        [SetUp]
        public void SetUp()
        {
            _schedulingRepository = new SchedulingRepository(_DbContext);
            _patientRepository = new PatientRepository(_DbContext);

            _schedulingValidator = new SchedulingRegisterValidator();
            _patientValidator = new PatientRegisterValidator();

            RegistrarObjeto(typeof(ISchedulingRepository), _schedulingRepository);
            RegistrarObjeto(typeof(IPatientRepository), _patientRepository);

            Registrar<ISchedulingBusiness, SchedulingBusiness>();
            Registrar<IPatientBusiness, PatientBusiness>();

            _schedulingBusiness = ObterServico<ISchedulingBusiness>();
            _patientBusiness = ObterServico<IPatientBusiness>();
        }

        [TestCase(0, 0)]
        [TestCase(0, 10)]
        public void GetAllScheduling_Success(int pageNumber, int pageSize)
        {
            async Task action() => await _schedulingBusiness.GetAllSchedulingOrderedByDateAndTime(pageNumber, pageSize);

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase(-1, 10)]
        [TestCase(0, -1)]
        public void GetAllScheduling_Invalid_PaginationParams(int pageNumber, int pageSize)
        {
            async Task action() => await _schedulingBusiness.GetAllSchedulingOrderedByDateAndTime(pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MinLength, pageNumber < 0 ? "pageNumber" : "pageSize", 0));
        }

        [TestCase(0, 1)]
        public async Task GetAllSchedulingByPatientd_Success(int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 1",
                Login = "PacienteTeste1",
                Email = "PacienteTeste1@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByPatientIdOrderedByDateAndTime(patientCreated.Id, pageNumber, pageSize);

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase(-1, 10)]
        public async Task GetAllSchedulingByPatient_Invalid_PageNumberParams(int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 2",
                Login = "PacienteTeste2",
                Email = "PacienteTeste2@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByPatientIdOrderedByDateAndTime(patientCreated.Id, pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MinLength, pageNumber < 0 ? "pageNumber" : "pageSize", 0));
        }

        [TestCase(0, -1)]
        public async Task GetAllSchedulingByPatient_Invalid_PageSizeParams(int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 11",
                Login = "PacienteTeste11",
                Email = "PacienteTeste11@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByPatientIdOrderedByDateAndTime(patientCreated.Id, pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MinLength, pageNumber < 0 ? "pageNumber" : "pageSize", 0));
        }

        [TestCase("Agendado", 0, 10)]

        public async Task GetAllSchedulingByStatusAgendado_Success(string status, int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 24",
                Login = "PacienteTeste24",
                Email = "PacienteTeste24@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, patientCreated.Id, pageNumber, pageSize);

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase("Concluído", 0, 10)]

        public async Task GetAllSchedulingByStatusConcluido_Success(string status, int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 30",
                Login = "PacienteTeste30",
                Email = "PacienteTeste30@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, patientCreated.Id, pageNumber, pageSize);

            Assert.DoesNotThrowAsync(action);
        }

        [TestCase("Cancelado", 0, 10)]

        public async Task GetAllSchedulingByStatusCancelado_Success(string status, int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 31",
                Login = "PacienteTeste31",
                Email = "PacienteTeste31@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, patientCreated.Id, pageNumber, pageSize);

            Assert.DoesNotThrowAsync(action);
        }

        public async Task GetAllSchedulingByStatus_Invalid_PatientId()
        {
            var invalidPatientId = 0;
            var status = StatusEnum.Agendado;
            var pageNumber = 0;
            var pageSize = 10;

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, invalidPatientId, pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<RegisterNotFound>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueNotFound, invalidPatientId));
        }

        [TestCase("Agendado", -1, -1)]

        public async Task GetAllSchedulingByStatus_Invalid_PaginationParams(string status, int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 21",
                Login = "PacienteTeste21",
                Email = "PacienteTeste21@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, patientCreated.Id, pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MinLength, pageNumber < 0 ? "pageNumber" : "pageSize", 0));
        }

        [TestCase("Concluído", 0, -1)]

        public async Task GetAllSchedulingByStatus_Invalid_pageSize(string status, int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 25",
                Login = "PacienteTeste25",
                Email = "PacienteTeste25@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, patientCreated.Id, pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MinLength, pageNumber < 0 ? "pageNumber" : "pageSize", 0));
        }

        [TestCase("Cancelado", -1, 10)]

        public async Task GetAllSchedulingByStatus_Invalid_PageNumberParams(string status, int pageNumber, int pageSize)
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 26",
                Login = "PacienteTeste26",
                Email = "PacienteTeste26@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, patientCreated.Id, pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MinLength, pageNumber < 0 ? "pageNumber" : "pageSize", 0));
        }

        [Test]
        public async Task GetAllSchedulingByStatus_Invalid()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 22",
                Login = "PacienteTeste22",
                Email = "PacienteTeste22@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var status = "Teste";
            int pageNumber = 0;
            int pageSize = 10;

            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, patientCreated.Id, pageNumber, pageSize);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, status));
        }

        [Test]
        public async Task SchedulingCompleted_Success()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 3",
                Login = "PacienteTeste3",
                Email = "PacienteTeste3@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            var result = _schedulingValidator.TestValidate(scheduling);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingDate);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingTime);

            var schedulingCreated = await _schedulingBusiness.SchedulingRegister(scheduling);

            var statusModel = new HandleStatusModel { 
                ScheduleId = schedulingCreated.Id,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCompleted(statusModel);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task SchedulingCompleted_InvalidaSchedulingId()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 4",
                Login = "PacienteTeste4",
                Email = "PacienteTeste4@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            var result = _schedulingValidator.TestValidate(scheduling);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingDate);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingTime);

            var schedulingCreated = await _schedulingBusiness.SchedulingRegister(scheduling);

            var notExistSchedulingId = 0;
            var statusModel = new HandleStatusModel
            {
                ScheduleId = notExistSchedulingId,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCompleted(statusModel);

            var excepetion = Assert.ThrowsAsync<RegisterNotFound>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueNotFound, notExistSchedulingId));
        }

        [Test]
        public async Task SchedulingCompleted_InvalidaStatus_WhenItIsAlready()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 13",
                Login = "PacienteTeste13",
                Email = "PacienteTeste13@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new Scheduling
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Concluído,
                CreateAt = DateTime.Now,
            };

            var schedulingCreated = await _schedulingRepository.Save(scheduling);
            var statusModel = new HandleStatusModel
            {
                ScheduleId = schedulingCreated.Id,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCompleted(statusModel);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Concluído));
        }

        [Test]
        public async Task SchedulingCompleted_InvalidaStatus_WhenItIsDeleted()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 5",
                Login = "PacienteTeste5",
                Email = "PacienteTeste5@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new Scheduling
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Cancelado,
                CreateAt = DateTime.Now,
            };

            var schedulingCreated = await _schedulingRepository.Save(scheduling);
            var statusModel = new HandleStatusModel
            {
                ScheduleId = schedulingCreated.Id,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCompleted(statusModel);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Cancelado));
        }

        [Test]
        public async Task SchedulingCanceled_Success()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 6",
                Login = "PacienteTeste6",
                Email = "PacienteTeste6@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            var result = _schedulingValidator.TestValidate(scheduling);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingDate);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingTime);

            var schedulingCreated = await _schedulingBusiness.SchedulingRegister(scheduling);

            var statusModel = new HandleStatusModel
            {
                ScheduleId = schedulingCreated.Id,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCanceled(statusModel);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task SchedulingDeleted_InvalidaSchedulingId()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 14",
                Login = "PacienteTeste14",
                Email = "PacienteTeste14@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var notExistSchedulingId = 0;
            var statusModel = new HandleStatusModel
            {
                ScheduleId = notExistSchedulingId,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCanceled(statusModel);

            var excepetion = Assert.ThrowsAsync<RegisterNotFound>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueNotFound, notExistSchedulingId));
        }

        [Test]
        public async Task SchedulingCanceled_InvalidaStatus_WhenItIsAlready()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 7",
                Login = "PacienteTeste7",
                Email = "PacienteTeste7@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new Scheduling
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Concluído,
                CreateAt = DateTime.Now,
            };

            var schedulingCreated = await _schedulingRepository.Save(scheduling);
            var statusModel = new HandleStatusModel
            {
                ScheduleId = schedulingCreated.Id,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCanceled(statusModel);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Concluído));
        }

        [Test]
        public async Task SchedulingCanceled_InvalidaStatus_WhenItIsDeleted()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 8",
                Login = "PacienteTeste8",
                Email = "PacienteTeste8@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new Scheduling
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Cancelado,
                CreateAt = DateTime.Now,
            };

            var schedulingCreated = await _schedulingRepository.Save(scheduling);
            var statusModel = new HandleStatusModel
            {
                ScheduleId = schedulingCreated.Id,
                PatientId = patientCreated.Id
            };

            async Task action() => await _schedulingBusiness.SchedulingCanceled(statusModel);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Cancelado));
        }


        [Test]
        public async Task SchedulingRegister_Success()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 12",
                Login = "PacienteTeste12",
                Email = "PacienteTeste12@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientCreated.Id,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            var result = _schedulingValidator.TestValidate(scheduling);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingDate);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingTime);

            async Task action() => await _schedulingBusiness.SchedulingRegister(scheduling);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task SchedulingRegister_Invalid_MaximumSchedulingAmountPerDay()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 9",
                Login = "PacienteTeste9",
                Email = "PacienteTeste9@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var time = DateTime.Now.TimeOfDay;
            var date = DateTime.Now;

            List<Scheduling> schedulings = new();
            var maximumSchedulingAmountPerDay = 20;

            for (int i = 0; i < maximumSchedulingAmountPerDay; i++)
            {
                var scheduling = new Scheduling
                {
                    PatientId = patientCreated.Id,
                    SchedulingDate = date,
                    SchedulingTime = time.Add(TimeSpan.FromMinutes((i / 2) * 15)),
                    Status = StatusEnum.Agendado,
                    CreateAt = DateTime.Now,
                };

                schedulings.Add(scheduling);
            }

            await _schedulingRepository.SaveAll(schedulings);

            var conflictScheduling = new SchedulingRegisterModel
            {
                PatientId = patientCreated.Id,
                SchedulingDate = date,
                SchedulingTime = time
            };

            var result = _schedulingValidator.TestValidate(conflictScheduling);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingDate);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingTime);

            async Task action() => await _schedulingBusiness.SchedulingRegister(conflictScheduling);

            var excepetion = Assert.ThrowsAsync<BusinessException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MaximumSchedulingQuantity, "dia", date));
        }

        [Test]
        public async Task SchedulingRegister_Invalid_MaximumSchedulingAmountPerTime()
        {
            var time = DateTime.Now.TimeOfDay;
            var date = DateTime.Now;

            List<Scheduling> schedulings = new();
            var maximumSchedulingAmountPerTime = 2;

            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 15",
                Login = "PacienteTeste15",
                Email = "PacienteTeste15@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            for (int i = 0; i < maximumSchedulingAmountPerTime; i++)
            {
                var scheduling = new Scheduling
                {
                    PatientId = patientCreated.Id,
                    SchedulingDate = date,
                    SchedulingTime = time,
                    Status = StatusEnum.Agendado,
                    CreateAt = DateTime.Now,
                };

                schedulings.Add(scheduling);
            }

            await _schedulingRepository.SaveAll(schedulings);

            var conflictScheduling = new SchedulingRegisterModel
            {
                PatientId = patientCreated.Id,
                SchedulingDate = date,
                SchedulingTime = time
            };

            var result = _schedulingValidator.TestValidate(conflictScheduling);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingDate);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingTime);

            async Task action() => await _schedulingBusiness.SchedulingRegister(conflictScheduling);

            var excepetion = Assert.ThrowsAsync<BusinessException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MaximumSchedulingQuantity, "hora", time));
        }

        [Test]
        public async Task SchedulingRegister_Invalid_Date()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Paciente Teste 10",
                Login = "PacienteTeste10",
                Email = "PacienteTeste10@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            var patientResult = _patientValidator.TestValidate(patient);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Name);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Login);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Email);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.Password);
            patientResult.ShouldNotHaveValidationErrorFor(patient => patient.BirthDate);

            var patientCreated = await _patientBusiness.SavePatient(patient);

            var time = DateTime.Now.TimeOfDay;
            var date = DateTime.Now.AddDays(-1).Date;

            var conflictScheduling = new SchedulingRegisterModel
            {   
                PatientId = patientCreated.Id,
                SchedulingDate = date,
                SchedulingTime = time
            };

            var result = _schedulingValidator.TestValidate(conflictScheduling);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingDate);
            result.ShouldNotHaveValidationErrorFor(patient => patient.SchedulingTime);

            async Task action() => await _schedulingBusiness.SchedulingRegister(conflictScheduling);

            var excepetion = Assert.ThrowsAsync<BusinessException>(action);
            Assert.IsTrue(excepetion.Message == BusinessMessages.InvalidDate);
        }


        [Test]
        public void GetAllHoursAvaliables_Success()
        {
            async Task action() => await _schedulingBusiness.HoursAvailable(DateTime.Now);

            Assert.DoesNotThrowAsync(action);
        }
    }
}
