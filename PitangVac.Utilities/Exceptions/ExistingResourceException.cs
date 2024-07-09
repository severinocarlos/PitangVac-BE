namespace PitangVac.Utilities.Exceptions
{
    public class ExistingResourceException : Exception
    {
        public ExistingResourceException() { }
        public ExistingResourceException(string message) : base(message) { }
    }
}
s