namespace DotNetRestApi.Models;

public record MineModel(
    string Message, 
    int Index, 
    List<Transaction> Transactions, 
    int Proof, 
    string PreviousHash
);
