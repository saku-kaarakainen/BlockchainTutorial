using Newtonsoft.Json;
namespace DotNetRestApi.Models;

public record Transaction(
    string Sender,
    string Recipent,
    decimal Amount);

public record Block(
    int Index,
    DateTimeOffset Timestamp,
    List<Transaction> Transactions,
    int Proof,
    string? PreviousHash = null) { 
    public string ToJson(Formatting formatting = Formatting.Indented) => JsonConvert.SerializeObject(this, formatting);    
}