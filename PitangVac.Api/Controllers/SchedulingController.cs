using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Attributes;

namespace PitangVac.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulingController : ControllerBase
    {
        private readonly ISchedulingBusiness _schedulingBusiness;

        public SchedulingController(ISchedulingBusiness schedulingBusiness)
        {
            _schedulingBusiness = schedulingBusiness;
        }


        [HttpGet]
        [Authorize]
        public async Task<SchedulingPaginationDTO> All([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return await _schedulingBusiness.GetAllSchedulingOrderedByDateAndTime(pageNumber, pageSize);
        }

        [HttpPost]
        [Transactional]
        [Authorize]
        public async Task<SchedulingDTO> SaveScheduling(SchedulingRegisterModel scheduling)
        {
            return await _schedulingBusiness.SchedulingRegister(scheduling);
        }

        [HttpGet("{patientId}")]
        [Authorize]
        public async Task<SchedulingPaginationDTO> AllByPatientId(int patientId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return await _schedulingBusiness.GetSchedulingsByPatientIdOrderedByDateAndTime(patientId, pageNumber, pageSize);
        }

        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<SchedulingPaginationDTO> AllByStatus(string status, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status, pageNumber, pageSize);
        }

        [HttpPost("status/complete")]
        [Transactional]
        [Authorize]
        public async Task<SchedulingDTO> CompleteScheduling(HandleStatusModel schedule)
        {
            return await _schedulingBusiness.SchedulingCompleted(schedule);
        }

        [HttpPost("status/cancel")]
        [Transactional]
        [Authorize]
        public async Task<SchedulingDTO> CancelScheduling(HandleStatusModel schedule)
        {
            return await _schedulingBusiness.SchedulingCanceled(schedule);
        }

        [HttpGet("hours-avaliable/{date}")]
        [Authorize]
        public async Task<List<string>> GetHoursAvaliable(DateTime date)
        {
            return await _schedulingBusiness.HoursAvailable(date);
        }
    }
}
