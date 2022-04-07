using DotNetRestApi.Models;
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using System.Net;

namespace DotNetRestApi.Services;
public class Nodes
{
    public string LocalNodeGuid { get; }
    public IPAddress LocalIpAddress { get; }

    public List<string> Neighbours { get; }

    private readonly Blockchain blockchain;
    private readonly HttpClient httpClient;

    public Nodes(
        IHttpContextAccessor httpContextAccessor,
        HttpClient httpClient,
        Blockchain blockchain)
    {
        this.httpClient = httpClient;
        this.blockchain = blockchain;

        // Generate a globally unique address for this node
        LocalNodeGuid = Guid.NewGuid()
            .ToString()
            .Replace("-", "");

        HttpContext httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        IHttpConnectionFeature httpConnectionFeature = httpContext.Features.Get<IHttpConnectionFeature>()!;
        LocalIpAddress = httpConnectionFeature?.LocalIpAddress ?? throw new InvalidOperationException("Unable to set the note.");
        Neighbours = new();
    }

    public void RegisterNode(string ipAddress)
    {
        this.Neighbours.Add(ipAddress);
    }

    /// <summary>
    /// This is our Consensus Algorithm, 
    /// it resolves conflicts by replacing our chain with the longest one in the network.
    /// </summary>
    /// <returns>True if our chain was replaced, False if not</returns>
    public async Task<bool> ResolveConflicts()
    {
        bool replaced = false;

        // We're only looking for chains longer than ours
        int maxLength = this.blockchain.Chain.Count;

        foreach(string node in Neighbours)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"https://{node}/chain");
            if(!response.IsSuccessStatusCode)
            {
                Debug.Print("Registered faulty node at: " + node);
                continue;
            }

            string content = await response.Content.ReadAsStringAsync();
            ChainModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<ChainModel>(content)!;

            // Check if the length is longer and the chain is valid
            if(model.Length > maxLength && IsValidChain(model.Chain))
            {
                maxLength = model.Length;
                this.blockchain.Chain = model.Chain;
                replaced = true;
                Debug.Print("Our chain was replaced with the chain from: " + node);
            }
        }

        return replaced;
    }

    /// <summary>
    /// Determine if a given blockchain is valid.
    /// </summary>
    /// <param name="chain">A blockchain</param>
    /// <returns>True if valid, False if not</returns>
    public bool IsValidChain(List<Block> chain)
    {
        /*
        last_block = chain[0]
        current_index = 1

        while current_index < len(chain):
            block = chain[current_index]
            print(f'{last_block}')
            print(f'{block}')
            print("\n-----------\n")
            # Check that the hash of the block is correct
            if block['previous_hash'] != self.hash(last_block):
                return False

            # Check that the Proof of Work is correct
            if not self.valid_proof(last_block['proof'], block['proof']):
                return False

            last_block = block
            current_index += 1

        return True
         */
        throw new NotFiniteNumberException();
    }
}

