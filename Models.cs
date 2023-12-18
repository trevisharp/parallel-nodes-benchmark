using System.Threading.Tasks;
using System.Collections.Concurrent;

public class NavyModel : Model
{
    bool isRunning = true;
    ConcurrentQueue<Node> queue = new ConcurrentQueue<Node>();

    public override void Add(Node node)
        => queue.Enqueue(node);

    public override async Task Run()
    {
        while (isRunning)
        {
            bool dequeued = queue.TryDequeue(out Node node);
            if (!dequeued)
                continue;
            
            this.NodeCount++;

            await Task.Run(() => node.Run(this));
        }
    }

    public override void Stop()
        => this.isRunning = false;
}