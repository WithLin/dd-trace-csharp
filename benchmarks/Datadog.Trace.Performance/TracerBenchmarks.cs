﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Datadog.Trace;

namespace MyBenchmarks
{
    public class TracerBenchmarks
    {
        [Benchmark]
        public void StartSpan()
        {
            using (var span = Tracer.Instance.StartSpan("Operation"))
            {
            }
        }

        [Benchmark]
        public void StartScope()
        {
            using (var scope = Tracer.Instance.StartActive("Operation"))
            {
            }
        }
    }
}