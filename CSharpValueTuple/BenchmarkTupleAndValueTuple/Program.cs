using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

namespace BenchmarkTupleAndValueTuple
{
    // https://adamsitnik.com/Value-Types-vs-Reference-Types/
    class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run<NoGC>();
    }

}

