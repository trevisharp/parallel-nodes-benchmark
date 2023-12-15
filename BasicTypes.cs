using System;
using System.Collections.Generic;

public abstract class Model
{
    public abstract void Add(Node node);
    
    public abstract void Run();
}

public class Node
{
    public TriggerType TriggerType { get; set; }
    public OperationSize Size { get; set; }
    public OperationType Type { get; set; }

    public Node Parent { get; set; }
    public List<Node> TriggerChildren { get; set; }
    
    public void Run(Model model)
    {
        var rnd = Random.Shared;
        int N = Size switch {
            OperationSize.Medium => rnd.Next(10, 20),
            OperationSize.Large => rnd.Next(50, 100),
            _ => rnd.Next(2, 4)
        };

        for (int i = 0; i < N; i++)
        {
            
        }
    }
}

public enum TriggerType
{
    RealTime,
    TimeBased,
    TriggerBased
}

public enum OperationSize
{
    Small,
    Medium,
    Large
}

public enum OperationType
{
    Blocking = 1,
    NonBlocking = 2,
    Mixed = 3
}