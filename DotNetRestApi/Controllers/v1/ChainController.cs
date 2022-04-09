using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ChainController : ControllerBase
{
    private readonly Blockchain blockchain;

    public ChainController(Blockchain blockchain)
    {
        this.blockchain = blockchain;
    }

    [HttpGet("")]
    public IActionResult Get() => base.Ok(new ChainModel(    
        Chain: this.blockchain.Chain,
        Length: this.blockchain.Chain.Count
    ));
    
}
