using PitangVac.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace PitangVac.Repository
{
    public class TransactionManagement : ITransactionManagement
    {
        private readonly DatabaseContext _databaseContext;

        public TransactionManagement(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            var activeTransaction = _databaseContext.Database.CurrentTransaction;
            if (activeTransaction == null)
            {
                var connection = _databaseContext.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var transaction = await connection.BeginTransactionAsync(isolationLevel);
                await _databaseContext.Database.UseTransactionAsync(transaction);
            }
        }

        public async Task CommitTransactionsAsync()
        {
            var contextHasChanges = _databaseContext.ChangeTracker.HasChanges();

            if (contextHasChanges)
                await _databaseContext.SaveChangesAsync();

            var activeTransaction = _databaseContext.Database.CurrentTransaction;
            await activeTransaction.CommitAsync();
            await activeTransaction.DisposeAsync();
        }

        public async Task RollbackTransactionsAsync()
        {
            var activeTransaction = _databaseContext.Database.CurrentTransaction;
            if (activeTransaction != null)
            {
                await activeTransaction.RollbackAsync();
                await activeTransaction.DisposeAsync();
            }
        }
    }
}
