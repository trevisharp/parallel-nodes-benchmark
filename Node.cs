using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Node
{
    public NodeSize Size { get; set; }
    public ProcessType Type { get; set; }

    public List<Node> MainChildren { get; set; } = new();
    public List<Node> TriggerChildren { get; set; } = new();
    
    public virtual void Run(Model model)
    {
        var rnd = Random.Shared;
        int N = Size switch {
            NodeSize.Large => rnd.Next(500, 1000),
            NodeSize.Medium => rnd.Next(50, 100),
            NodeSize.Small => rnd.Next(5, 10),
            _ => 1
        };
        
        if ((this.Type & ProcessType.NonBlocking) > 0)
            for (int n = 0; n < N * 100_000; n++) ;
            
        if ((this.Type & ProcessType.Blocking) > 0)
            Thread.Sleep(N);
        
        foreach (var node in this.MainChildren)
            model.Add(node);
        
        foreach (var node in this.TriggerChildren)
            if (rnd.NextSingle() < .1f)
                model.Add(node);
    }
}

public class StopNode : Node
{
    public override async void Run(Model model)
    {
        await Task.Delay(2000);
        model.Stop();
    }
}

public enum NodeSize
{
    Minimal,
    Small,
    Medium,
    Large
}

public enum ProcessType
{
    Blocking = 1,
    NonBlocking = 2,
    Mixed = Blocking | NonBlocking
}