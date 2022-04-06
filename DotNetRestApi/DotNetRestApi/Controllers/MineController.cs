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
    private readonly Node node;
    private readonly Cryptograph cryptograph;

    public MineController(
        Blockchain blockchain,
        ConsensusMechanism consensusMechanism,
        Node node,
        Cryptograph cryptograph)
    {
        this.blockchain = blockchain;
        this.consensusMechanism = consensusMechanism;
        this.node = node;
        this.cryptograph = cryptograph;
    }

    private async Task<Block> ForgeGenesisBlock()
    {
        int proof = await Task.Run(() => this.consensusMechanism.ProofOfWork(0));

        // We must receive a reward for finding the proof.
        // The sender is "0" to signify that this node has mined a new coin.
        this.blockchain.NewTransaction(
            sender: "0",
            recipient: node.GloballyUniqueAddress,
            amount: 1 // One btc, or whatever will be our currency
        );

        Block block = this.blockchain.NewBlock(proof, null);
        return block;
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        // We run the proof of work algorithm to get the next proof...
        if(!this.blockchain.Chain.Any())
        {
            Block genesis = await ForgeGenesisBlock();
            return Ok(new MineModel(
                "Gensis Block forged", 
                genesis.Index, 
                genesis.Transactions, 
                genesis.Proof, 
                genesis.PreviousHash ?? ""
            ));
        }

        Block lastBlock = this.blockchain.LastBlock();
        int lastProof = lastBlock.Proof;
        int proof = await Task.Run(() => this.consensusMechanism.ProofOfWork(lastProof));

        // We must receive a reward for finding the proof.
        // The sender is "0" to signify that this node has mined a new coin.
        this.blockchain.NewTransaction(
            sender: "0",
            recipient: node.GloballyUniqueAddress,
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