using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class NodesController : ControllerBase
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly Blockchain blockchain;
    private readonly Nodes _nodes;
    public NodesController(
        IHttpContextAccessor httpContextAccessor,
        Blockchain blockchain, 
        Nodes nodes)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.blockchain = blockchain;
        this._nodes = nodes;
    }

    [HttpPost("register")]
    public IActionResult Register(List<string> nodes)
    {
        bool nodesAdded = false;
        foreach (string node in nodes)
        {
            if (this._nodes.RegisterNode(node))
            {
                nodesAdded = true;
            }
        }

        return nodesAdded
            ? StatusCode(201, new RegisterNodeModel("New nodes have been added in the request.", this._nodes.NodesCount))
            : StatusCode(200, new RegisterNodeModel("No new nodes in the request.", this._nodes.NodesCount));
    }

    [HttpGet("resolve")]
    public async Task<IActionResult> Resolve()
    {
        bool isReplaced = await this._nodes.ResolveConflicts();

        if(isReplaced)
        {
            return Ok(new ResolveNewNodeModel(
                Message: "Our chain was replaced",
                NewChain: this.blockchain.Chain
            ));
        }
        else
        {
            return Ok(new ResolveOldNodeModel(            
                Message: "Our chain was authoritative",
                Chain: this.blockchain.Chain
            ));
        }
    }
}

