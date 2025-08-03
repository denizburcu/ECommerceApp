using System.Threading.Tasks;
using ECommerceApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerceApp.Persistence;

/// <summary>
/// Manages transactions and saving changes as a unit.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IDataContext _dataContext;
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    public UnitOfWork(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    public async Task<int> SaveAsync()
    {
        return await ((DbContext)_dataContext).SaveChangesAsync();
    }

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        _transaction = await ((DbContext)_dataContext).Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}