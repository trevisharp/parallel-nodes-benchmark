using System;
 
const int N = 10_000;

Benchmark.Test<NavyModel, OceanScenario>(N);
Benchmark.Test<FirstModel, OceanScenario>(N);