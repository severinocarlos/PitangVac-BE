namespace PitangVac.Entity.DTO
{
    public class PatientTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public PatientTokenDTO(string token, string refreshToken) 
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
