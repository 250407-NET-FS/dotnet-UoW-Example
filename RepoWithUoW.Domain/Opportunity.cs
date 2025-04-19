namespace RepoWithUoW.Domain;


public class Opportunity 
{
    // do not set as required, or endpoints will have trouble
    // auto-generating the GUIDs
    public string Id { get; set; } = Guid.NewGuid().ToString();

    
    public required string Name { get; set; }

    
    // required foreign key property convention for EF to setup 1-N
    // non-nullable types are default for maximum participation, nullable types for optional participation
    // since we have the required keyword, maximum participation will be enforced and cascade delete is enabled:
    
    public required string AccountId { get; set; }

    // this is necessarily required, but it helps EF traverse the relationship/setup AccountId foreign key
    public Account? account { get; set; }
}