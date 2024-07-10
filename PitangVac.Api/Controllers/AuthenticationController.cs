using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;

namespace PitangVac.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;

        public AuthenticationController(IAuthenticationBusiness authenticationBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
        }

        [HttpPost("login")]
        public async Task<PatientTokenDTO> Login([FromBody] LoginModel login)
        {
            return await _authenticationBusiness.Login(login);
        }

        [HttpGet("refresh-token")]
        [Authorize]
        public async Task<PatientTokenDTO> RefreshToken()
        {
            return await _authenticationBusiness.RefreshToken();
        }
    }
}
