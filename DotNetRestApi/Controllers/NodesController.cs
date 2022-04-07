using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NodesController : ControllerBase
{
    private readonly Blockchain blockchain;
    private readonly Nodes _nodes;
    public NodesController(Blockchain blockchain, Nodes nodes)
    {
        this.blockchain = blockchain;
        this._nodes = nodes;
    }

    [HttpPost("register")]
    public IActionResult Register(List<string> nodes)
    {
        foreach(string node in nodes)
        {
            this._nodes.RegisterNode(node);
        }

        return StatusCode(201, new RegisterNodeModel("New nodes have been added", this._nodes.RegisteredNodes.Count));
    }

    [HttpGet("resolve")]
    public IActionResult Resolve()
    {
        bool isReplaced = this._nodes.ResolveConflicts();

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

