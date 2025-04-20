using RepoWithUoW.Domain;

namespace RepoWithUoW.Repo;

public interface IOpportunityRepo : IDisposable
{

    
    public  Task<Opportunity> AddOpportunityAsync(Opportunity opp);

    public Task<Opportunity>  DeleteOpportunityAsync(Opportunity opp);

    public Task<List<Opportunity>> GetAllOpportunitiesAsync();

    public Task<List<Opportunity>> AddOpportunityListAsync(List<Opportunity> oppList);

    public Task Save();
}
