// rationale for multiple DbContexts:
// https://www.milanjovanovic.tech/blog/using-multiple-ef-core-dbcontext-in-single-application

using RepoWithUoW.Service;
using RepoWithUoW.Repo;
using Microsoft.EntityFrameworkCore;
using RepoWithUoW.Domain;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RepoWithUoW.API;

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
builder.Services.AddScoped<IAuthService, AuthService_Impl>();

// unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddIdentityCore<User>(options => 
{

    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AccountDbContext>()
.AddSignInManager<SignInManager<User>>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
builder.Services.AddAuthentication(options => 
{
   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
   
})
.AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // ensures that we can't create tokens with expiration in the past
    };

     options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Example: Read token from cookies or custom headers
            if (context.Request.Cookies.ContainsKey("AuthToken"))
            {
                context.Token = context.Request.Cookies["AuthToken"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options => 
{
    options.AddPolicy(
        "Development",
        policy => 
        {
            policy.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        }
    );
});

var app = builder.Build();


app.UseHttpsRedirection();
app.UseCors("Development");
//app.UseAuthentication();

app.UseAuthorization();

//Below UseAuthorization, Im going to call my RolesInitializer's method
//and seed my user roles. After this runs once, I will comment it out to avoid any issues.

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;

//     try
//     {
//         await RolesInitalizer.SeedRoles(services);
//     }
//     catch (Exception ex)
//     {
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "Error seeding roles");
//     }
// }


app.MapControllers();



app.Run();