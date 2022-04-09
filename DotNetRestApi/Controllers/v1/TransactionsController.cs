using DotNetRestApi.Models;
using DotNetRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetRestApi.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly Blockchain blockchain;

    public TransactionsController(Blockchain blockchain)
    {
        this.blockchain = blockchain;
    }


    [HttpPost("New")]
    public IActionResult New(string sender, string recipient, decimal amount)
    {
        int index = this.blockchain.NewTransaction(sender, recipient, amount);
        return Ok(new TransactionModel(
            Message: "Transaction will be added to Block " + index
        ));
    }
}