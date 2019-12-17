using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace TestFrameworksProject
{
    public class xUnitUnitTest
    {
        public List<int> list = new List<int>();

        public List<int> listSetup = new List<int>();

        public List<int> listOneTimeSetup = new List<int>();
        private ITestOutputHelper output;

        public xUnitUnitTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        public void TestA(int i)
        {
            list.Add(i);
            listSetup.Add(i);
            listOneTimeSetup.Add(i);
            output.WriteLine(list.Count.ToString());
            output.WriteLine(listOneTimeSetup.Count.ToString());
            Assert.Single(listSetup);
            Assert.Equal(list.Count, listOneTimeSetup.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        public void TestB(int i)
        {
            list.Add(i);
            listSetup.Add(i);
            listOneTimeSetup.Add(i);
            output.WriteLine(list.Count.ToString());
            output.WriteLine(listOneTimeSetup.Count.ToString());
            Assert.Single(listSetup);
            Assert.Equal(list.Count, listOneTimeSetup.Count);
        }
    }
}
