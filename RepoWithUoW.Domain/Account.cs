using System.ComponentModel.DataAnnotations;

namespace RepoWithUoW.Domain;

public class Account
{
    // need to leave required keyword off so we can auto-generate GUIDs properly 
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }


// This isn't necessarily required, but will help EF generate migrations and understand the relationship
// EF will set up 1-N relationship, i.e. one account can have many opportunities, 
// and there is maximum participation for each opportunity, partial for accounts 
// lazy loading this:
    private ICollection<Opportunity>? _Opportunities;
    public ICollection<Opportunity> Opportunities => _Opportunities ??= new List<Opportunity>();

}
