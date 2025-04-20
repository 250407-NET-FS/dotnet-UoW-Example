using System.Transactions;

namespace RepoWithUoW.Repo;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IAccountRepo _accRepo;
    private readonly IOpportunityRepo _oppRepo;

    private TransactionScope _transaction;

    public UnitOfWork(IAccountRepo accRepo, IOpportunityRepo oppRepo)
    {
        _accRepo = accRepo;
        _oppRepo = oppRepo;
    }

    // Public getters for repositories
    public IAccountRepo AccountRepo => _accRepo;
    public IOpportunityRepo OpportunityRepo => _oppRepo;
    
     public async Task BeginTransaction()
    {
        _transaction = new TransactionScope();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _accRepo.Save();
            await _oppRepo.Save();
            _transaction.Complete();
        }
        catch
        {
            throw new Exception("Transaction rolled back, some records failed!");
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}