using DotNetRestApi.Models;
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using System.Net;

namespace DotNetRestApi.Services;
public class Nodes
{    
    private readonly Blockchain blockchain;
    private readonly Cryptograph cryptograph;
    private readonly HttpClient httpClient;
    private List<string> neighbours;

    public string LocalNodeGuid { get; }
    public IPAddress LocalIpAddress { get; }


    public Nodes(
        //IHttpContextAccessor httpContextAccessor,
        HttpClient httpClient,
        Blockchain blockchain,
        Cryptograph cryptograph)
    {
        this.httpClient = httpClient;
        this.blockchain = blockchain;
        this.cryptograph = cryptograph;

        // Generate a globally unique address for this node
        LocalNodeGuid = Guid.NewGuid()
            .ToString()
            .Replace("-", "");

        //HttpContext httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        //IHttpConnectionFeature httpConnectionFeature = httpContext.Features.Get<IHttpConnectionFeature>()!;
        //LocalIpAddress = httpConnectionFeature?.LocalIpAddress ?? throw new InvalidOperationException("Unable to set the note.");
        neighbours = new();
    }

    public bool RegisterNode(string ipAddress)
    {
        if(this.neighbours.Contains(ipAddress))
        {
            Debug.Print($"The node '{ipAddress}' is already added");
            return false;
        }

        this.neighbours.Add(ipAddress);
        return true;
    }

    public int NodesCount => neighbours.Count;

    /// <summary>
    /// This is our Consensus Algorithm, 
    /// it resolves conflicts by replacing our chain with the longest one in the network.
    /// </summary>
    /// <returns>True if our chain was replaced, False if not</returns>
    public async Task<(bool replaced, string replacingNode)> ResolveConflicts()
    {
        bool replaced = false;
        string replacingNode = "https://127.0.0.1";

        // We're only looking for chains longer than ours
        int maxLength = this.blockchain.Chain.Count;

        foreach(string node in neighbours)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{node}/Chain");
            if (!response.IsSuccessStatusCode)
            {
                Debug.Print("Cannot fetch chain data from node: " + node);
                return default;
            }

            string content = await response.Content.ReadAsStringAsync();
            ChainModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<ChainModel>(content)!;

            // Check if the length is longer and the chain is valid
            if (model.Length > maxLength)
            {
                if (IsValidChain(model.Chain))
                {
                    maxLength = model.Length;
                    this.blockchain.Chain = model.Chain;
                    replaced = true;
                    replacingNode = node;
                    Debug.Print("Our chain was replaced with the chain from: " + node);
                }
            }
        }

        return (replaced, replacingNode);
    }

    /// <summary>
    /// Determine if a given blockchain is valid.
    /// </summary>
    /// <param name="chain">A blockchain</param>
    /// <returns>True if valid, False if not</returns>
    public bool IsValidChain(List<Block> chain)
    {
        if (!chain.Any())
            return true; // empty is valid

        Block block;
        Block lastBlock = chain.First();
        string lastBlockHash = cryptograph.CreateHash(lastBlock);
        Debug.Print("Last block hash: " + lastBlockHash);
        Debug.Print("Last block: " + lastBlock.ToJson());

        int currentIndex = 1;

        while(currentIndex < chain.Count)
        {
            block = chain.ElementAt(currentIndex);
            Debug.Print($"block at current index ({currentIndex}): " + block.ToJson());

            // Check that the hash of the block is correct
            if (block.PreviousHash != cryptograph.CreateHash(lastBlock))
            {
                Debug.Print("Previous hash doesn't match up with the last block hash. Returing false.");
                return false;
            }

            lastBlock = block;
            currentIndex++;
        }

        return true;
    }
}

