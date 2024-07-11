using PitangVac.Entity.Enums;

namespace PitangVac.Validators.Manual
{
    public static class StatusValidator
    {
        public static bool IsValidStatus(string status)
        {
            return StatusEnum.Status.Contains(status);
        }
    }
}
