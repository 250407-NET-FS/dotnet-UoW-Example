using RepoWithUoW.Domain;
using RepoWithUoW.Repo;

namespace RepoWithUoW.Service;

public interface IAppService
{

    public Task<Account> AddAccountAsync(Account acc);

    public Task<Account> DeleteAccountAsync(Account acc);

    public Task<List<Account>> GetAccountsAsync();

    public Task<Opportunity> AddOpportunityAsync(Opportunity opp);

    public Task<Opportunity> DeleteOpportunityAsync(Opportunity opp);

    public Task<List<Opportunity>> GetOpportunitiesAsync();

    public Task<Dictionary<Account, List<Opportunity>>> NewAccountOnboarding(Account acc, Opportunity opp);


}