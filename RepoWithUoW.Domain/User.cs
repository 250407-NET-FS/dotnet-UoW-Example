using Microsoft.AspNetCore.Identity;

namespace RepoWithUoW.Domain;

public class User : IdentityUser
{
    // the parent class already has a lot of common stuff
    // we may need for a user. So we only need to add things 
    // if needed

    public string? FullName { get; set; }
}
