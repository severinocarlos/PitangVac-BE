using System.Data;

namespace PitangVac.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TransactionalAttribute : Attribute
    {
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

        public TransactionalAttribute() { }

        public TransactionalAttribute(IsolationLevel isolationLevel)
        {
            IsolationLevel = isolationLevel;
        }
        
    }
}
