// rationale for multiple DbContexts:
// https://www.milanjovanovic.tech/blog/using-multiple-ef-core-dbcontext-in-single-application

using RepoWithUoW.Service;
using RepoWithUoW.Repo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

//Loading the string from my env file - HINT: There are other ways to do this
//Things like Secrets, AppSettings (don't forget to edit your gitignore for this one)
// and packages like DotNetEnv to load from envs more easily

string conn_string = File.ReadAllText("./conn_string.env");

// these are obviously connecting to the exact same server but for the scenario
// we can pretend they may be a distributed system where we need to keep 
// relational state between them (theres many reasons to want to do this, check the link up top)
builder.Services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(conn_string));
builder.Services.AddDbContext<SalesDbContext>(options => options.UseSqlServer(conn_string));


// repositories
builder.Services.AddScoped<IAccountRepo, AccountRepo_Impl>();
builder.Services.AddScoped<IOpportunityRepo, OpportunityRepo_Impl>();


// services 
builder.Services.AddScoped<IAppService, AppService_Impl>();

// unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();