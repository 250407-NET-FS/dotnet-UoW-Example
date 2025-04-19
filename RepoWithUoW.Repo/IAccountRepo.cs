using RepoWithUoW.Domain;

namespace RepoWithUoW.Repo;

public interface IAccountRepo
{

    public Task<Account> AddAccountAsync(Account acc);

    public Task<Account> DeleteAccountAsync(Account acc);

    public Task<List<Account>> GetAllAccountsAsync();

    public Task Save();
}
