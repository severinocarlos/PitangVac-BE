
using PitangVac.Entity.DTO;
using PitangVac.Entity.Models;

namespace PitangVac.Business.Interface.IBusiness
{
    public interface IAuthenticationBusiness
    {
        Task<PatientTokenDTO> Login(LoginModel login);
        Task<PatientTokenDTO> RefreshToken();
    }
}
