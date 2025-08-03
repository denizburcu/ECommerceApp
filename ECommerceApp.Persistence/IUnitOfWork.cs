namespace ECommerceApp.Persistence;

using System.Threading.Tasks;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
}