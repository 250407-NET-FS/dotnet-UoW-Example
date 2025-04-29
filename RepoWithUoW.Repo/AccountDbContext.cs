using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepoWithUoW.Domain;


namespace RepoWithUoW.Repo;

public class AccountDbContext : IdentityDbContext<User, IdentityRole, string>
{
    //Your DbContext needs a constructor - this constructor takes a special argument of type
    //DbContextOptions - we need to call the base (parent) constructor since it is the one
    // who uses the options argument
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options) { }

    //We need to add our entities, that EF will track for us
    //We do that by creating DbSet objects
    public DbSet<Account> Account { get; set; }

}
