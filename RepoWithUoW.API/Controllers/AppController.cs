namespace  RepoWithUoW.API;

using Microsoft.AspNetCore.Mvc;
using RepoWithUoW.Service;
using RepoWithUoW.Domain;

[ApiController]
[Route("[controller]")]
public class AppController : ControllerBase
{

    private readonly IAppService _service;

    public AppController(IAppService service)
    {
        _service = service;
    }

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

    [HttpPost("onboarding")]
    public async Task<IActionResult> Onboarding(HttpRequest request)
    {
        try
        {
            // Extract parameters from the query string
            string accName = request.Query["AccountName"]!;
            string oppName = request.Query["OpportunityName"]!;


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