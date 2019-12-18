using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestFrameworksProject
{
    [TestFixture]
    public class NunitUnitTest
    {
        public List<int> list;

        public List<int> listSetup;

        public List<int> listOneTimeSetup;

        public NunitUnitTest() => list = new List<int>();

        [OneTimeSetUp]
        public void OneTimeInit() => listOneTimeSetup = new List<int>();

        [SetUp]
        public void Init() => listSetup = new List<int>();

        [Parallelizable]
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        public void TestA(int i)
        {
            list.Add(i);
            listSetup.Add(i);
            listOneTimeSetup.Add(i);
            Console.WriteLine(list.Count);
            Console.WriteLine(listOneTimeSetup.Count);
            Assert.AreEqual(1, listSetup.Count);
            Assert.AreEqual(list.Count, listOneTimeSetup.Count);
        }

        [Parallelizable(ParallelScope.All)]
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        public void TestB(int i)
        {
            list.Add(i);
            listSetup.Add(i);
            listOneTimeSetup.Add(i);
            Console.WriteLine(list.Count);
            Console.WriteLine(listOneTimeSetup.Count);
            Assert.AreEqual(1, listSetup.Count);
            Assert.AreEqual(list.Count, listOneTimeSetup.Count);
        }
    }
}