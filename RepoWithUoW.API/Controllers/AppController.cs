namespace  RepoWithUoW.API;

using Microsoft.AspNetCore.Mvc;
using RepoWithUoW.Service;
using RepoWithUoW.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

[ApiController]
[Route("[controller]")]
public class AppController : ControllerBase
{

    private readonly IAppService _service;

    public AppController(IAppService service)
    {
        _service = service;
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")]
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAllAccounts()
    {
        try
        {
            List<Account> accList = await _service.GetAccountsAsync();
            return Ok(accList.Select(a => a.ToDTO()).ToList());
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")] 
    [HttpPost("account")]
    public async Task<IActionResult> CreateAccount(Account acc)
    {
        try
        {
            Account account = await _service.AddAccountAsync(acc);
            return Ok(account.ToDTO());
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")]
    [HttpDelete("account")]
    public async Task<IActionResult> DeleteAccount(Account acc)
    {
        try
        {
            var account = await _service.DeleteAccountAsync(acc);
            return Ok(acc.ToDTO());
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")]
    [HttpGet("opportunities")]
    public async Task<IActionResult> GetAllOpportunities()
    {
        try
        {
            List<Opportunity> opps = await _service.GetOpportunitiesAsync();
            return Ok(opps.Select(o => o.ToDTO()).ToList());
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")]
    [HttpPost("opportunity")]
    public async Task<IActionResult> CreateOpportunity(Opportunity opp)
    {
        
        try
        {
            Opportunity opportunity = await _service.AddOpportunityAsync(opp);
            return Ok(opportunity.ToDTO());
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")]
    [HttpDelete("opportunity")]
    public async Task<IActionResult> DeleteOpportunity(Opportunity opp)
    {
        try
        {
            Opportunity opportunity = await _service.DeleteOpportunityAsync(opp);
            return Ok(opportunity.ToDTO());
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")]
    [HttpPost("onboarding")]
    public async Task<IActionResult> Onboarding()
    {
        try
        {
            // Access the HttpRequest object directly from HttpContext
            var routeValues = HttpContext.Request.RouteValues;
            var accName = HttpContext.Request.Query["AccountName"].ToString();
            var oppName = HttpContext.Request.Query["OpportunityName"].ToString();
           

            Account acc = new() { Name = accName };
            Opportunity opp = new() { Name = oppName, AccountId = acc.Id };

            await _service.NewAccountOnboarding(acc, opp);
            
            return Ok(acc.ToDTO());

        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}