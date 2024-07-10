namespace PitangVac.Entity.Models
{
    public class SchedulingRegisterModel
    {
        public int PatientId { get; set; }
        public DateTime SchedulingDate { get; set; }
        public TimeSpan SchedulingTime { get; set; }
        public string Status { get; set; }
    }
}
