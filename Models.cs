using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class NavyModel : Model
{
    bool isRunning = true;
    ConcurrentQueue<Node> queue = new ConcurrentQueue<Node>();

    public override void Add(Node node)
        => queue.Enqueue(node);

    public override void Run()
    {
        while (isRunning)
        {
            bool dequeued = queue.TryDequeue(out Node node);
            if (!dequeued)
                continue;
            
            this.NodeCount++;

            ThreadPool.QueueUserWorkItem(_ => node.Run(this));
        }
    }

    public override void Stop()
        => this.isRunning = false;
}