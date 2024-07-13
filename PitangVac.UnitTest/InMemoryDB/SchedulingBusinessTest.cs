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
using System;

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

        [Test]
        public void GetAllScheduling_Success()
        {
            async Task action() => await _schedulingBusiness.GetAllSchedulingOrderedByDateAndTime();

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task GetAllSchedulingByPatient_Success()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);


            var patientId = 1;
            async Task action() => await _schedulingBusiness.GetSchedulingsByPatientIdOrderedByDateAndTime(patientId);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task GetAllSchedulingByPatient_NotExist()
        {
            var patientId = 1;
            async Task action() => await _schedulingBusiness.GetSchedulingsByPatientIdOrderedByDateAndTime(patientId);

            var excepetion = Assert.ThrowsAsync<RegisterNotFound>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueNotFound, patientId));
        }

        [TestCase("Agendado")]
        [TestCase("Concluído")]
        [TestCase("Cancelado")]

        public async Task GetAllSchedulingByStatus_Success(string status)
        { 
            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task GetAllSchedulingByStatus_Invalid()
        {
            var status = "Teste";
            async Task action() => await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, status));
        }

        [Test]
        public async Task SchedulingCompleted_Success()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            await _schedulingBusiness.SchedulingRegister(scheduling);

            var schedulingId = 1;

            async Task action() => await _schedulingBusiness.SchedulingCompleted(schedulingId);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task SchedulingCompleted_InvalidaSchedulingId()
        {
            var schedulingId = 1;

            async Task action() => await _schedulingBusiness.SchedulingCompleted(schedulingId);

            var excepetion = Assert.ThrowsAsync<RegisterNotFound>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueNotFound, schedulingId));
        }

        [Test]
        public async Task SchedulingCompleted_InvalidaStatus_WhenItIsAlready()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;

            var scheduling = new Scheduling
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Concluído,
                CreateAt = DateTime.Now,
            };

            var schedulingId = 1;

            await _schedulingRepository.Save(scheduling);

            async Task action() => await _schedulingBusiness.SchedulingCompleted(schedulingId);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Concluído));
        }

        [Test]
        public async Task SchedulingCompleted_InvalidaStatus_WhenItIsDeleted()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;

            var scheduling = new Scheduling
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Cancelado,
                CreateAt = DateTime.Now,
            };

            await _schedulingRepository.Save(scheduling);

            var schedulingId = 1;

            async Task action() => await _schedulingBusiness.SchedulingCompleted(schedulingId);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Cancelado));
        }

        [Test]
        public async Task SchedulingCanceled_Success()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            await _schedulingBusiness.SchedulingRegister(scheduling);

            var schedulingId = 1;

            async Task action() => await _schedulingBusiness.SchedulingCanceled(schedulingId);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task SchedulingDeleted_InvalidaSchedulingId()
        {
            var schedulingId = 1;

            async Task action() => await _schedulingBusiness.SchedulingCanceled(schedulingId);

            var excepetion = Assert.ThrowsAsync<RegisterNotFound>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueNotFound, schedulingId));
        }

        [Test]
        public async Task SchedulingCanceled_InvalidaStatus_WhenItIsAlready()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;

            var scheduling = new Scheduling
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Concluído,
                CreateAt = DateTime.Now,
            };

            await _schedulingRepository.Save(scheduling);

            var schedulingId = 1;

            async Task action() => await _schedulingBusiness.SchedulingCanceled(schedulingId);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Concluído));
        }

        [Test]
        public async Task SchedulingCanceled_InvalidaStatus_WhenItIsDeleted()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;

            var scheduling = new Scheduling
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay,
                Status = StatusEnum.Cancelado,
                CreateAt = DateTime.Now,
            };

            await _schedulingRepository.Save(scheduling);

            var schedulingId = 1;

            async Task action() => await _schedulingBusiness.SchedulingCanceled(schedulingId);

            var excepetion = Assert.ThrowsAsync<InvalidDataException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.InvalidValue, StatusEnum.Cancelado));
        }


        [Test]
        public async Task SchedulingRegister_Success()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            async Task action() => await _schedulingBusiness.SchedulingRegister(scheduling);

            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public async Task SchedulingRegister_Invalid_PatientNotExist()
        {
            var patientId = 1;

            var scheduling = new SchedulingRegisterModel
            {
                PatientId = patientId,
                SchedulingDate = DateTime.Now,
                SchedulingTime = DateTime.Now.TimeOfDay
            };

            async Task action() => await _schedulingBusiness.SchedulingRegister(scheduling);

            var excepetion = Assert.ThrowsAsync<RegisterNotFound>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.ValueNotFound, patientId));
        }


        [Test]
        public async Task SchedulingRegister_Invalid_MaximumSchedulingAmountPerDay()
        {
            var patient = new PatientRegisterModel
            {
                Name = "Teste",
                Login = "Teste",
                Email = "Test@teste.com",
                Password = "Test",
                BirthDate = DateTime.Now,
            };

            await _patientBusiness.SavePatient(patient);

            var patientId = 1;
            var time = DateTime.Now.TimeOfDay;
            var date = DateTime.Now;

            List<Scheduling> schedulings = new List<Scheduling>();
            var maximumSchedulingAmountPerDay = 20;

            for (int i = 0; i < maximumSchedulingAmountPerDay; i++)
            {
                var scheduling = new Scheduling
                {
                    PatientId = patientId,
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
                PatientId = patientId,
                SchedulingDate = date,
                SchedulingTime = time
            };

            var schedluingCreated = _schedulingBusiness.GetAllSchedulingOrderedByDateAndTime();

            async Task action() => await _schedulingBusiness.SchedulingRegister(conflictScheduling);

            var excepetion = Assert.ThrowsAsync<BusinessException>(action);
            Assert.IsTrue(excepetion.Message == string.Format(BusinessMessages.MaximumSchedulingQuantity, "dia", date));
        }
    }
}
