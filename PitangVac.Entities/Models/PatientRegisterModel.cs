namespace PitangVac.Entity.Models
{
    public class PatientRegisterModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
