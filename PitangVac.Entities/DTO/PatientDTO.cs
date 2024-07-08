namespace PitangVac.Entity.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
