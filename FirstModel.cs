using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class FirstModel : Model
{
    int activeCount = 0;
    bool isRunning = false;
    AutoResetEvent stopSignal = new AutoResetEvent(false);
    AutoResetEvent queueSignal = new AutoResetEvent(false);
    ConcurrentQueue<Node> queue = new ConcurrentQueue<Node>();

    public override void Add(Node node)
    {
        if (!isRunning)
        {
            queue.Enqueue(node);
            return;
        }

        if (activeCount > 12 * Environment.ProcessorCount)
        {
            queue.Enqueue(node);
            if (queue.Count > 40)
                queueSignal.Set();
            return;
        }

        if (activeCount < Environment.ProcessorCount && queue.Count > 0)
            queueSignal.Set();
        
        execute(node);
    }

    public override void Run()
    {
        isRunning = true;

        Task.Run(() => {
            while (isRunning)
            {
                if (queue.Count == 0)
                    queueSignal.WaitOne();
                
                bool dequeued = queue.TryDequeue(out Node node);
                if (!dequeued)
                    continue;
                
                execute(node);
            }
        });

        stopSignal.WaitOne();
    }

    public override void Stop()
    {
        this.isRunning = false;
        stopSignal.Set();
    }

    void execute(Node node)
    {
        Task.Run(() => {
            activeCount++;
            node.Run(this);
            activeCount--;
        });
    }
}