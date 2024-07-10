using PitangVac.Business.Interface.IBusiness;
using PitangVac.Entity.DTO;
using PitangVac.Entity.Entities;
using PitangVac.Entity.Models;
using PitangVac.Utilities.Configurations;
using PitangVac.Utilities.UserContext;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using PitangVac.Utilities.Messages;
using PitangVac.Utilities.Extensions;


namespace PitangVac.Business.Business
{
    public class AuthenticationBusiness : IAuthenticationBusiness
    {

        private readonly IPatientBusiness _patientBusiness;
        private readonly IUserContext _userContext;
        private readonly AuthenticationConfig _authenticationConfig;

        public AuthenticationBusiness(IPatientBusiness patientBusiness, 
                                      IUserContext userContext,
                                      IOptionsMonitor<AuthenticationConfig> authenticationConfig)
        {
            _patientBusiness = patientBusiness;
            _userContext = userContext;
            _authenticationConfig = authenticationConfig.CurrentValue;
        }

        public async Task<PatientTokenDTO> Login(LoginModel login)
        {
            var patient = await _patientBusiness.FindPatientByLogin(login.Login) ?? throw new UnauthorizedAccessException(BusinessMessages.UserPasswordInvalid);
            
            
            using var hmac = new HMACSHA512(patient.PasswordSalt);

            var passwordIsCorrect = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password))
                                                    .SequenceEqual(patient.PasswordHash);

            if (!passwordIsCorrect)
            {
                throw new UnauthorizedAccessException(BusinessMessages.UserPasswordInvalid);
            }
            
            var token = GenerateToken(patient);
            var refreshToken = GenerateRefreshToken(patient);

            return new PatientTokenDTO(token, refreshToken);
        }

        public async Task<PatientTokenDTO> RefreshToken()
        {
            var login = _userContext.Login();
            var patient = await _patientBusiness.FindPatientByLogin(login) ?? throw new UnauthorizedAccessException(BusinessMessages.UserPasswordInvalid);

            var token = GenerateToken(patient);
            var refreshToken = GenerateRefreshToken(patient);

            return new PatientTokenDTO(token, refreshToken);
        }

        public string GenerateToken(Patient patient)
        {
            var expiration = DateTime.Now.AddMinutes(_authenticationConfig.AccessTokenExpiration);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, patient.Id.ToString()),
                new(ClaimTypes.Name, patient.Name),
                new("login", patient.Login),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfig.SecretKey));
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _authenticationConfig.Issuer,
                audience: _authenticationConfig.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(Patient patient)
        {
            var expiration = DateTime.Now.AddMinutes(_authenticationConfig.RefreshTokenExpiration);

            var claims = new List<Claim>
            {
                new("login", patient.Login)
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfig.SecretKey));
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _authenticationConfig.Issuer,
                audience: _authenticationConfig.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
