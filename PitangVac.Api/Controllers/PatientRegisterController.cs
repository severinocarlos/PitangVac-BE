using Microsoft.AspNetCore.Mvc;
using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Attributes;

namespace PitangVac.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PatientRegisterController : ControllerBase
    {
        private readonly IPatientBusiness _patientBusiness;

        public PatientRegisterController(IPatientBusiness patientBusiness)
        {
            _patientBusiness = patientBusiness;
        }

        [HttpPost("register")]
        [Transactional]
        public async Task<PatientDTO> SavePatient(PatientModel patientModel)
        {
            return await _patientBusiness.SavePatient(patientModel);
        }
    }
}
