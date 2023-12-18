using System.Diagnostics;
using System.Collections.Generic;

public abstract class Scenario
{
    public int[] Run<M>(params int[] sizes)
        where M : Model, new()
    {
        List<int> count = new List<int>();
        var sw = new Stopwatch();

        foreach (var size in sizes)
        {
            var model = new M();
            
            load(model, size);
            
            sw.Restart();
            model.Run();
            count.Add((int)sw.ElapsedMilliseconds);
        }

        return count.ToArray();
    }
    
    protected abstract void load(Model model, int nodeCount);
}