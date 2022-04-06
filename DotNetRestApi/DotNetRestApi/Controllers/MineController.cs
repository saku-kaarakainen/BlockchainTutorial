using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers;
[ApiController]
[Route("[controller]")]
public class MineController : ControllerBase
{
    private readonly Blockchain blockchain;
    private readonly ConsensusMechanism consensusMechanism;
    public MineController(
        Blockchain blockchain,
        ConsensusMechanism consensusMechanism)
    {
        this.blockchain = blockchain;
        this.consensusMechanism = consensusMechanism;
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        // We run the proof of work algorithm to get the next proof...
        Block lastBlock = this.blockchain.LastBlock();
        int lastProof = lastBlock.Proof;
        int proof = await Task.Run(() => this.consensusMechanism.ProofOfWork(lastProof));

        // TODO: 
        return base.StatusCode(501, "proof: " + proof);
    }
}


