namespace PitangVac.Entity.Entities
{
    public class Patient
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreateAt { get; set; }
        public List<Scheduling> Schedulings { get; set; }

        public Patient() { }
    }
}
