using System.Data;

namespace PitangVac.Repository.Interface
{
    public interface ITransactionManagement
    {
        Task BeginTransactionAsync(IsolationLevel isolationLevel);
        Task CommitTransactionsAsync();
        Task RollbackTransactionsAsync();
    }
}
