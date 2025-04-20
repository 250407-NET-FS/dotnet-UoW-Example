using RepoWithUoW.Domain;
using Microsoft.EntityFrameworkCore;


namespace RepoWithUoW.Repo;

public class OpportunityRepo_Impl : IOpportunityRepo, IDisposable
{

    private readonly SalesDbContext _context;

    public OpportunityRepo_Impl(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<Opportunity> AddOpportunityAsync(Opportunity opp)
    {
        await _context.Opportunity.AddAsync(opp);
        return opp;
    }

    public async Task<Opportunity> DeleteOpportunityAsync(Opportunity opp)
    {
        _context.Opportunity.Remove(opp);
        return opp;
    }

    public async Task<List<Opportunity>> GetAllOpportunitiesAsync()
    {
        return await _context.Opportunity.ToListAsync();
    }

    public async Task<List<Opportunity>> AddOpportunityListAsync(List<Opportunity> oppList)
    {
        foreach(Opportunity opp in oppList)
        {
            await _context.AddAsync(opp);
        }
        return oppList;
    }

     public async Task Save() 
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
