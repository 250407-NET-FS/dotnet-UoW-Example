// rationale for multiple DbContexts:
// https://www.milanjovanovic.tech/blog/using-multiple-ef-core-dbcontext-in-single-application


using RepoWithUoW.Service;
using RepoWithUoW.Domain;
using RepoWithUoW.Repo;
using Microsoft.EntityFrameworkCore;

// we need to have the FromBody on each of the endpoints that take in a body
// if even one is missing, we will get errors later about inferred body params
// apparently in .NET 7+ minimal APIs no longer infer complex types from the body by default 
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

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


// get all methods for each entity type
app.MapGet(
    "/accounts", 
    async (IAppService service) => 
    {
        List<Account> accList = await service.GetAccountsAsync();
        return Results.Ok(accList.Select(a => a.ToDTO()).ToList());
    }
);

app.MapGet(
    "/opportunities", 
    async (IAppService service) => 
    {
        List<Opportunity> oppList = await service.GetOpportunitiesAsync();
        return Results.Ok(oppList.Select(o => o.ToDTO()).ToList());
    }
);


// create individual post method for each entity type
app.MapPost(
    "/account",
    async ( [FromBody] Account acc, IAppService service) =>
    {
        await service.AddAccountAsync(acc);
        return Results.Created($"/account/{acc.Id}", new AccountDTO(){
            Id = acc.Id,
            Name = acc.Name
        });
    }
);

app.MapPost(
    "/opportunity",
    async ( [FromBody] Opportunity opp, IAppService service) =>
    {
        await service.AddOpportunityAsync(opp);
        return Results.Created($"/opportunity/{opp.Id}", new OpportunityDTO(){
            Id = opp.Id,
            Name = opp.Name,
            AccountId = opp.AccountId
        });
    }
);


// delete individual post methods for each entity type
app.MapDelete(
    "/account",
    async ( [FromBody] Account acc, IAppService service) =>
    {
        await service.DeleteAccountAsync(acc);
        return Results.Ok(new AccountDTO(){
            Id = acc.Id,
            Name = acc.Name
        });
    }

);

app.MapDelete(
    "/opportunity",
    async ( [FromBody] Opportunity opp, IAppService service) =>
    {
        await service.DeleteOpportunityAsync(opp);
        return Results.Ok(new OpportunityDTO(){
            Id = opp.Id,
            Name = opp.Name,
            AccountId = opp.AccountId
        });
    }

);


// onboarding method. creates parent and child record in one operation 
// FromBody only works for one parameter at most. So we need to use the 
// HttpRequest object and parse out everything manually
// this demonstrates data inconsistency without a unit of work
// We can call this endpoint and the account will always insert even if the opp fails to insert
// this is resolved now with our unit of work:
app.MapPost(
    "/onboarding",
    async ( HttpRequest request, IAppService service) =>
    {
         // Extract parameters from the query string
        string accName = request.Query["AccountName"]!;
        string oppName = request.Query["OpportunityName"]!;

        Account acc = new() { Name = accName };
        Opportunity opp = new() { Name = oppName, AccountId = acc.Id };

        await service.NewAccountOnboarding(acc, opp);

        return Results.Created($"/account/{acc.Id}", new AccountDTO()
        {
            Id = acc.Id,
            Name = acc.Name
        });

    }

);


app.Run();







// app.MapGet(
//     "/students/{id}",
//     async (int id, IStudentService service) =>
//     {
//         var student = await service.GetByIdAsync(id);
//         return student is not null ? Results.Ok(student) : Results.NotFound();
//     }
// );