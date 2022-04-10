using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class MineController : ControllerBase
{
    private readonly Blockchain blockchain;
    private readonly ConsensusMechanism consensusMechanism;
    private readonly Nodes nodes;
    private readonly Cryptograph cryptograph;

    public MineController(
        Blockchain blockchain,
        ConsensusMechanism consensusMechanism,
        Nodes nodes,
        Cryptograph cryptograph)
    {
        this.blockchain = blockchain;
        this.consensusMechanism = consensusMechanism;
        this.nodes = nodes;
        this.cryptograph = cryptograph;
    }

    /// <summary>
    /// Mine a new block with Proof of Work. 
    /// The difficulty is purposely set to really easy, 
    /// just that you can see how it works.
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        Block lastBlock = this.blockchain.LastBlock();
        int lastProof = lastBlock.Proof;
        int proof = await Task.Run(() => this.consensusMechanism.ProofOfWork(lastProof));

        // We must receive a reward for finding the proof.
        // The sender is "0" to signify that this node has mined a new coin.
        this.blockchain.NewTransaction(
            sender: "0",
            recipient: nodes.LocalNodeGuid,
            amount: 1 // One btc, or whatever will be our currency
        );

        // Forge the new Block by adding it to the chain
        string previousHash = this.cryptograph.CreateHash(lastBlock);
        Block block = this.blockchain.NewBlock(proof, previousHash);

        return Ok(new MineModel(
            "New Block forged", 
            block.Index, 
            block.Transactions, 
            proof, 
            previousHash));
    }
}