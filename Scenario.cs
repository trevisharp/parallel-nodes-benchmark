using System.Threading.Tasks;
using System.Collections.Generic;

public abstract class Scenario
{
    public async Task<int[]> Run<M>(params int[] sizes)
        where M : Model, new()
    {
        List<int> count = new List<int>();

        foreach (var size in sizes)
        {
            var model = new M();
            
            model.Add(new StopNode());
            load(model, size);
            await model.Run();
            
            count.Add(model.NodeCount);
        }

        return count.ToArray();
    }
    
    protected abstract void load(Model model, int nodeCount);
}