using BenchmarkDotNet.Running;
using System;

namespace ReadonlyStructMethod
{
    class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run<ReadonlyStructMethod>();
    }
}
