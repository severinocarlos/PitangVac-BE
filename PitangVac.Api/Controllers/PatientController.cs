﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Attributes;

namespace PitangVac.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientBusiness _patientBusiness;

        public PatientController(IPatientBusiness patientBusiness)
        {
            _patientBusiness = patientBusiness;
        }

        [HttpPost("register")]
        [Transactional]
        public async Task<PatientDTO> SavePatient([FromBody] PatientRegisterModel patient)
        {
            return await _patientBusiness.SavePatient(patient);
        }

        [HttpGet("patients/{patientName}")]
        [Authorize]
        public async Task<List<PatientDTO>> GetPatientsByName(string patientName)
        {
            return await _patientBusiness.FindPatientByName(patientName);
        }
    }
}
