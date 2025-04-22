namespace RepoWithUoW.Service;

using RepoWithUoW.Domain;
using RepoWithUoW.Repo;


public class AppService_Impl : IAppService
{

    private readonly IUnitOfWork _unitOfWork;

    public AppService_Impl(IUnitOfWork unitOfWork) 
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Account> AddAccountAsync(Account acc)
    {
        await _unitOfWork.BeginTransaction();
        await _unitOfWork.AccountRepo.AddAccountAsync(acc);
        await _unitOfWork.CommitAsync();
        return acc;
    }

    public async Task<Account> DeleteAccountAsync(Account acc)
    {
        await _unitOfWork.BeginTransaction();
        await _unitOfWork.AccountRepo.DeleteAccountAsync(acc);
        await _unitOfWork.CommitAsync();
        return acc;
    }

    public async Task<List<Account>> GetAccountsAsync()
    {
        return await _unitOfWork.AccountRepo.GetAllAccountsAsync();
    }

    public async Task<Opportunity> AddOpportunityAsync(Opportunity opp)
    {
        await _unitOfWork.BeginTransaction();
        await _unitOfWork.OpportunityRepo.AddOpportunityAsync(opp);
        await _unitOfWork.CommitAsync();
        return opp;
    }

    public async Task<Opportunity> DeleteOpportunityAsync(Opportunity opp)
    {
        await _unitOfWork.BeginTransaction();
        await _unitOfWork.OpportunityRepo.DeleteOpportunityAsync(opp);
        await _unitOfWork.CommitAsync();
        return opp;
    }

    public async Task<List<Opportunity>> GetOpportunitiesAsync()
    {   
        return await _unitOfWork.OpportunityRepo.GetAllOpportunitiesAsync();
    }


    public async Task<Account> GetAccountById(string id)
    {
        return await _unitOfWork.AccountRepo.GetAccountById(id);
    }

     public async Task<Opportunity> GetOpportunityById(string id)
    {
        return await _unitOfWork.OpportunityRepo.GetOpportunityById(id);
    }

    public async Task<Dictionary<Account, List<Opportunity>>> NewAccountOnboarding(Account acc, Opportunity opp) 
    {
        await _unitOfWork.BeginTransaction(); 
        await _unitOfWork.AccountRepo.AddAccountAsync(acc);
        await _unitOfWork.OpportunityRepo.AddOpportunityAsync(opp);
        await _unitOfWork.CommitAsync();

        return new Dictionary<Account, List<Opportunity>>()
        {
            { acc, new List<Opportunity>() { opp } }
        };
    }
}