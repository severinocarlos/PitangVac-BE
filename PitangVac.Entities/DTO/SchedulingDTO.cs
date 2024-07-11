namespace PitangVac.Entity.DTO
{
    public class SchedulingDTO
    {
        public int Id { get; set; }
        public DateTime SchedulingDate { get; set; }
        public TimeSpan SchedulingTime { get; set; }
        public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public PatientDTO Patient { get; set; }
    }
}
