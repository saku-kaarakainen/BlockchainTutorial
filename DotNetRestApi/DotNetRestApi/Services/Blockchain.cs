using Microsoft.Extensions.Internal;
using System.Diagnostics;
using DotNetRestApi.Models;

namespace DotNetRestApi.Services;

// Uses singleton pattern in the services
public class Blockchain 
{
    private readonly ISystemClock clock;
    private List<Transaction> currentTransactions;

    public Blockchain(ISystemClock clock)
    {
        this.clock = clock;
        Chain = new ();
        currentTransactions = new();
    }

    public List<Block> Chain { get; }

    /// <summary>
    /// Creates a new Block in the Blockchain
    /// </summary>
    /// <param name="proof">The proof given by the Proof of Work algorithm</param>
    /// <param name="previousHash">(Optional) Hash of previous Block</param>
    /// <returns>New Block</returns>
    public Block NewBlock(int proof, string? previousHash = null)
    {
        Block block = new(
            Index: this.Chain.Count + 1,
            Timestamp: clock.UtcNow,
            Transactions: currentTransactions,
            Proof: proof,
            PreviousHash: previousHash
        );

        Debug.Print("Resetting the current list of transactions");
        this.currentTransactions = new();

        this.Chain.Add(block);

        return block;
    }

    /// <summary>
    /// Creates a new transaction to go into the next mined Block
    /// </summary>
    /// <param name="sender">Address of the sender</param>
    /// <param name="recipent">Address of the recipient</param>
    /// <param name="amount">Amount</param>
    /// <returns> The index of the Block that will hold this transaction</returns>
    public int NewTransaction(string sender, string recipent, decimal amount)
    {
        this.currentTransactions.Add(new(sender, recipent, amount));

        if(!this.Chain.Any())
        {
            Debug.Print("Returing genesis block");
            return 1;
        }
        
        int index =this.Chain.Last().Index + 1;
        Debug.Print("New index: " + index);
        return index;
    }

    // Returns the last Block in the chain
    public Block LastBlock() => this.Chain.Last();
}

