using RepoWithUoW.Domain;
using Microsoft.EntityFrameworkCore;

namespace RepoWithUoW.Repo;

public class AccountRepo_Impl : IAccountRepo, IDisposable
{
  
    private readonly AccountDbContext _context;

    public AccountRepo_Impl(AccountDbContext context)
    {
        _context = context;
    }


    public async Task<List<Account>> GetAllAccountsAsync()
    {
        return await _context.Account.ToListAsync();
    }


    public async Task<Account> AddAccountAsync(Account acc) 
    {
        await _context.Account.AddAsync(acc);
        return acc;
    }

    public async Task<Account> DeleteAccountAsync(Account acc)
    {
        _context.Account.Remove(acc);
        return acc;
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
