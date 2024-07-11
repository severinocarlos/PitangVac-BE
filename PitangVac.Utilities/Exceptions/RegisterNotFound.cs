namespace PitangVac.Utilities.Exceptions
{
    public class RegisterNotFound : Exception
    {
        public RegisterNotFound() { }
        public RegisterNotFound(string message) : base(message) { }
    }
}
