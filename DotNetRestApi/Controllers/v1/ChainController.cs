using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class ChainController : ControllerBase
{
    private readonly Blockchain blockchain;

    public ChainController(Blockchain blockchain)
    {
        this.blockchain = blockchain;
    }

    /// <summary>
    /// Gets the current chain and it's length from the node.
    /// </summary>
    /// <returns>The chain and it's length.</returns>
    [HttpGet("")]
    public IActionResult Get() => base.Ok(new ChainModel(    
        Chain: this.blockchain.Chain,
        Length: this.blockchain.Chain.Count
    ));
}
