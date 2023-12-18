
using System.Threading.Tasks;

public abstract class Model
{
    public int NodeCount { get; protected set; }

    public abstract void Add(Node node);
    
    public abstract Task Run();

    public abstract void Stop();
}