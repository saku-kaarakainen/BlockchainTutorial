using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly Blockchain blockchain;

    public TransactionsController(Blockchain blockchain)
    {
        this.blockchain = blockchain;
    }


    /// <summary>
    /// Adds new transaction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="recipient"></param>
    /// <param name="amount"></param>
    /// <remarks>
    /// Notes: This service doesn't have any security, so you can easily fake a transaction.
    /// </remarks>
    [HttpPost("New")]
    public IActionResult New(string sender, string recipient, decimal amount)
    {
        int index = this.blockchain.NewTransaction(sender, recipient, amount);
        return Ok(new TransactionModel(
            Message: "Transaction will be added to Block " + index
        ));
    }
}