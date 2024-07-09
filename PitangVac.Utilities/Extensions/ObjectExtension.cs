namespace PitangVac.Utilities.Extensions
{
    public static class ObjectExtension
    {
        public static string? String(this object value)
        {
            return value == null ? string.Empty : value.ToString();
        }
    }
}
