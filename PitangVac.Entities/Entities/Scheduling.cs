namespace PitangVac.Entity.Entities
{
    public class Scheduling
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime SchedulingDate { get; set; }
        public TimeSpan SchedulingTime { get; set; }
        public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public Patient Patient { get; set; }

        public Scheduling() { }

    }
}
