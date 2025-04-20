namespace RepoWithUoW.Repo;

public interface IUnitOfWork : IDisposable
{

   IAccountRepo AccountRepo { get; }

   IOpportunityRepo OpportunityRepo { get; }
    public Task BeginTransaction();

    public Task CommitAsync();


}