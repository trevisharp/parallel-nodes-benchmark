using System;
using System.Threading.Tasks;

public static class Benchmark
{
    public static async Task Test<M, S>(params int[] sizes)
        where M : Model, new()
        where S : Scenario, new()
    {
        var scenario = new S();
        int[] fullRes = new int[sizes.Length];

        for (int i = 0; i < 5; i ++)
        {
            var results = await scenario.Run<M>(sizes);
            for (int n = 0; n < results.Length; n++)
                fullRes[n] += results[n];
        }

        for (int n = 0; n < fullRes.Length; n++)
        {
            Console.WriteLine($"{typeof(M).Name} on {typeof(S).Name}:");
            Console.WriteLine($"\t{sizes[n]} -> {fullRes[n] / 5}");
        }
    }
}