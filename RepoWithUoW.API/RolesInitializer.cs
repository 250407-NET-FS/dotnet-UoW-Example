//This logic will run once.
// //We create/use this AFTER we set up identity AND do a migration for that

using Microsoft.AspNetCore.Identity;
using RepoWithUoW.Domain;

namespace RepoWithUoW.API;

//Im making this class static, I don't want anything to create a RolesInitializer object
public static class RolesInitalizer 
{
    //This class will provide one static method to SeedRoles
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {   
            //If the role doesnt exist, we add it.
            //If it does, we dont want redundant role records. 
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var user = new User{
            UserName = "sa@test.com",
            Email = "sa@test.com",
            FullName = "sa"
        };
        
        await userManager.CreateAsync(user, "Password123!");
        await userManager.AddToRoleAsync(user, "Admin");
    }
}
