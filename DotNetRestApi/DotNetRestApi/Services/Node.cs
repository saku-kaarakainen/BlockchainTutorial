namespace DotNetRestApi.Services;
public class Node
{
    public string GloballyUniqueAddress { get; }
    
    public Node()
        =>
        // Generate a globally unique address for this node
        GloballyUniqueAddress = Guid.NewGuid()
            .ToString()
            .Replace("-", "");    
}

