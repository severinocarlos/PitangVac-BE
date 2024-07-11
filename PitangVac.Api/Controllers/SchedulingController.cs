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


    }
}
