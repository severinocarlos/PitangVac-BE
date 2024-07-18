﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<List<SchedulingDTO>> All()
        {
            return await _schedulingBusiness.GetAllSchedulingOrderedByDateAndTime();
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
        public async Task<List<SchedulingDTO>> AllByPatientId(int patientId)
        {
            return await _schedulingBusiness.GetSchedulingsByPatientIdOrderedByDateAndTime(patientId);
        }

        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<List<SchedulingDTO>> AllByStatus(string status)
        {
            return await _schedulingBusiness.GetSchedulingsByStatusOrderedByDateAndTime(status);
        }

        [HttpPost("status/complete")]
        [Transactional]
        [Authorize]
        public async Task<List<SchedulingDTO>> CompleteScheduling(HandleStatusModel schedule)
        {
            return await _schedulingBusiness.SchedulingCompleted(schedule.ScheduleId);
        }

        [HttpPost("status/cancel")]
        [Transactional]
        [Authorize]
        public async Task<List<SchedulingDTO>> CancelScheduling(HandleStatusModel schedule)
        {
            return await _schedulingBusiness.SchedulingCanceled(schedule.ScheduleId);
        }

        [HttpGet("hours-avaliable/{date}")]
        [Authorize]
        public async Task<List<string>> GetHoursAvaliable(DateTime date)
        {
            return await _schedulingBusiness.HoursAvailable(date);
        }
    }
}
