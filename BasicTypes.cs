public abstract class PANASModel
{
    protected List<Node> nodes = new List<Node>();
    public void Add(Node node)
        => nodes.Add(node);
    
    public abstract void Run();
}

public abstract class Node
{
    public NodeType Type { get; set; }
    public abstract void Run();
}

public enum NodeType
{
    OnLoop,
    OnTime,
    OnTrigger
}