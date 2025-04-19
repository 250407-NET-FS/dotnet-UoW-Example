namespace RepoWithUoW.Service;

using RepoWithUoW.Domain;
using RepoWithUoW.Repo;


public class AppService_Impl : IAppService
{

    private readonly IAccountRepo _accRepo;
    private readonly IOpportunityRepo _oppRepo;

    public AppService_Impl(IAccountRepo accRepo, IOpportunityRepo oppRepo) 
    {
        _accRepo = accRepo;
        _oppRepo = oppRepo;
    }

    public async Task<Account> AddAccountAsync(Account acc)
    {
        await _accRepo.AddAccountAsync(acc);
        await _accRepo.Save();
        return acc;
    }

    public async Task<Account> DeleteAccountAsync(Account acc)
    {
        
        await _accRepo.DeleteAccountAsync(acc);
        await _accRepo.Save();
        return acc;
    }

    public async Task<List<Account>> GetAccountsAsync()
    {
        return await _accRepo.GetAllAccountsAsync();
    }

    public async Task<Opportunity> AddOpportunityAsync(Opportunity opp)
    {
        await _oppRepo.AddOpportunityAsync(opp);
        await _oppRepo.Save();
        return opp;
    }

    public async Task<Opportunity> DeleteOpportunityAsync(Opportunity opp)
    {
        await _oppRepo.DeleteOpportunityAsync(opp);
        await _oppRepo.Save();
        return opp;
    }

    public async Task<List<Opportunity>> GetOpportunitiesAsync()
    {   
        return await _oppRepo.GetAllOpportunitiesAsync();
    }




    public async Task<Dictionary<Account, List<Opportunity>>> NewAccountOnboarding(Account acc, Opportunity opp) 
    {

        await _accRepo.AddAccountAsync(acc);
        await _accRepo.Save();


        await _oppRepo.AddOpportunityAsync(opp);
        await _oppRepo.Save();

        return new Dictionary<Account, List<Opportunity>>()
        {
            { acc, new List<Opportunity>() { opp } }
        };
    }
}