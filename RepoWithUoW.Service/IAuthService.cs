namespace RepoWithUoW.Service;
using RepoWithUoW.Domain;

public interface IAuthService
{

    public Task<string> GenerateToken(User u);
}