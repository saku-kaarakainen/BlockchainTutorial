namespace DotNetRestApi.Models;

public record RegisterNodeModel(string Message, int TotalNodes);

public record ResolveNewNodeModel(string Message, List<Block> NewChain);
public record ResolveOldNodeModel(string Message, List<Block> Chain);
