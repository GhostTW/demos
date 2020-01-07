using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadonlyStructMethod
{
	[SimpleJob(RuntimeMoniker.CoreRt31)]
	public class ReadonlyStructMethod
	{
		private int N;

		[GlobalSetup]
		public void Setup()
		{
			this.N = new Random(42).Next();
		}

		[Benchmark(Baseline = true)]
		[Arguments(100000000)]
		public void NonReadonlyProperty(int iter)
		{
			for (int i = 0; i < iter; i++)
			{
				var result = new Test(this.N).Modify();
			}
		}

		[Benchmark]
		[Arguments(100000000)]
		public void ReadonlyProperty(int iter)
		{
			for (int i = 0; i < iter; i++)
			{
				var result = new TestA(this.N).Modify();
			}
		}
	}


	public struct Test
	{
		public int Number;
		public int Square => Number + Number;
		public Test(int n)
		{
			this.Number = n;
		}

		public readonly int Modify()
		{
			return Square;
		}
	}

	public struct TestA
	{
		public readonly int Number;
		public readonly int Square => Number + Number;
		public TestA(int n)
		{
			this.Number = n;
		}

		public readonly int Modify()
		{
			return Square;
		}
	}

}
