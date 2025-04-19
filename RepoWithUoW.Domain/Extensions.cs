namespace RepoWithUoW.Domain;

public static class Extensions
{

    // adding extension methods to be used for List<Account> conversion to List<AccountDTO> 
    public static AccountDTO ToDTO(this Account account)
    {
        return new AccountDTO()
        {
            Id = account.Id,
            Name = account.Name
        };
    }


    // doing the same thing with opportunities too
    public static OpportunityDTO ToDTO(this Opportunity opportunity)
    {
        return new OpportunityDTO()
        {
            Id = opportunity.Id,
            Name = opportunity.Name,
            AccountId = opportunity.AccountId
        };
    }

}