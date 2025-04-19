// ended up using this DbContext to create the migration
// it is complete with relationship references and the uniqueness constraint on opp name

using Microsoft.EntityFrameworkCore;
using RepoWithUoW.Domain;

namespace RepoWithUoW.Repo;

public class SalesDbContext : DbContext
{
    //Your DbContext needs a constructor - this constructor takes a special argument of type
    //DbContextOptions - we need to call the base (parent) constructor since it is the one
    // who uses the options argument
    public SalesDbContext(DbContextOptions<SalesDbContext> options)
        : base(options) { }

    //We need to add our entities, that EF will track for us
    //We do that by creating DbSet objects
    public DbSet<Opportunity> Opportunity { get; set; }


    // making opp names unique for the demo
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enforce uniqueness on the Name property
        modelBuilder.Entity<Opportunity>()
            .HasIndex(o => o.Name)
            .IsUnique();
    }
}
