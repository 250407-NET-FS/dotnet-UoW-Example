namespace RepoWithUoW.Domain;

public class AccountDTO
{
   
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }


}
