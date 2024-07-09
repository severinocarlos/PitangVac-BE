namespace PitangVac.Utilities.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception exception) : base(message, exception) { }
    }
}
