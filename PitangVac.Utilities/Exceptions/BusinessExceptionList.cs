using System.Runtime.Serialization;

namespace PitangVac.Utilities.Exceptions
{
    public class BusinessExceptionList : Exception
    {
        public List<string> Messages { get; set; }

        public BusinessExceptionList() { }

        public BusinessExceptionList(IEnumerable<string> messages)
        {
            Messages = messages.ToList();
        }

        protected BusinessExceptionList(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
