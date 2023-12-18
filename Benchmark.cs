using System;
using System.Linq;

public static class Benchmark
{
    public static void Test<M, S>(params int[] sizes)
        where M : Model, new()
        where S : Scenario, new()
    {
        var scenario = new S();
        int[] fullRes = new int[sizes.Length];

        for (int i = 0; i < 5; i ++)
        {
            var loadBar = string.Concat(
                "12345".Select((c, j) => j < i ? '█' : ' ')
            );

            Console.Clear();
            Console.WriteLine("┌─────┐");
            Console.WriteLine($"│{loadBar}│");
            Console.WriteLine("└─────┘");

            var results = scenario.Run<M>(sizes);
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