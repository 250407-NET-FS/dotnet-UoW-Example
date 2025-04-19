namespace RepoWithUoW.Domain;


public class OpportunityDTO
{

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }
    
    public required string AccountId { get; set; }

}