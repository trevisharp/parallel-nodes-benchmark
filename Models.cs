using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class NavyModel : Model
{
    bool isRunning = true;
    ConcurrentQueue<Node> queue = new ConcurrentQueue<Node>();

    public override void Add(Node node)
    {
        if (!isRunning)
        {
            queue.Enqueue(node);
            return;
        }
        
        node.Run(this);
    }

    public override void Run()
    {
        isRunning = true;
        foreach (var node in queue)
            node.Run(this);
    }

    public override void Stop() { }
}

public class TaskModel : Model
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
            Task.Run(() => node.Run(this));
        }
    }

    public override void Stop()
        => this.isRunning = false;
}

public class SignalModel : Model
{
    AutoResetEvent signal = new AutoResetEvent(false);
    bool isRunning = true;
    ConcurrentQueue<Node> queue = new ConcurrentQueue<Node>();

    public override void Add(Node node)
    {
        queue.Enqueue(node);
        signal.Set();
    }

    public override void Run()
    {
        while (isRunning)
        {
            if (queue.Count == 0)
                signal.WaitOne(1);
            
            bool dequeued = queue.TryDequeue(out Node node);
            if (!dequeued)
                continue;
            
            this.NodeCount++;
            Task.Run(() => node.Run(this));
        }
    }

    public override void Stop()
        => this.isRunning = false;
}

public class NoQueueModel : Model
{
    bool isRunning = false;
    ConcurrentQueue<Node> startQueue = new ConcurrentQueue<Node>();

    public override void Add(Node node)
    {
        if (isRunning)
        {
            this.NodeCount++;
            Task.Run(() => node.Run(this));
            return;
        }
        
        startQueue.Enqueue(node);
    }

    public override void Run()
    {
        isRunning = true;
        while (startQueue.Count > 0)
        {
            bool dequeued = startQueue.TryDequeue(out Node node);
            if (!dequeued)
                break;
            
            Add(node);
        }

        while (isRunning)
            Thread.Sleep(50);
    }

    public override void Stop()
        => this.isRunning = false;
}

public class MultiConsumeModel : Model
{
    bool isRunning = true;
    ConcurrentQueue<Node> queue = new ConcurrentQueue<Node>();
    
    AutoResetEvent 
        s1 = new AutoResetEvent(false),
        s2 = new AutoResetEvent(false),
        s3 = new AutoResetEvent(false),
        s4 = new AutoResetEvent(false);

    public override void Add(Node node)
    {
        queue.Enqueue(node);

        s1.Set();
        if (queue.Count > 20)
            s2.Set();
        if (queue.Count > 80)
            s3.Set();
        if (queue.Count > 320)
            s4.Set();
    }

    public override void Run()
    {

        Parallel.Invoke(
            () => consume(null),
            () => consume(s1),
            () => consume(s2),
            () => consume(s2),
            () => consume(s3),
            () => consume(s3),
            () => consume(s4),
            () => consume(s4)
        );

        void consume(AutoResetEvent signal)
        {
            while (isRunning)
            {
                signal?.WaitOne(10);

                bool dequeued = queue.TryDequeue(out Node node);
                if (!dequeued)
                    continue;
                
                this.NodeCount++;
                node.Run(this);
            }
        }
    }

    public override void Stop()
        => this.isRunning = false;
}
