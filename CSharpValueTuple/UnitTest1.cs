using NUnit.Framework;
using System;

namespace CSharpValueTuple
{
    public class Program
    {

        public Tuple<string, int> SimpleTuple => new Tuple<string, int>("testTuple", 1);

        public (string, int) SimpleValueTuple => ("testValueTuple", 2);

        public (string Name, int Value) ValueTupleName => (Name: "testValueTupleName", Value: 3);

        public ValueTuple<string, int> TypeValueTyple => (Name: "testTypeValueTup", Value: 4);

        public ValueTuple<string, int> TypeValueTypleA => ValueTuple.Create("testTypeValueTup", 4);
    }

    public class Tests
    {
        private Program _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Program();
        }

        [Test]
        public void TypeCheck()
        {
            Assert.IsTrue(_sut.SimpleTuple.GetType() == typeof(Tuple<string, int>));
            Assert.IsTrue(_sut.SimpleValueTuple.GetType() == typeof(ValueTuple<string, int>));
            Assert.IsTrue(_sut.ValueTupleName.GetType() == typeof(ValueTuple<string, int>));
            Assert.IsTrue(_sut.TypeValueTyple.GetType() == typeof(ValueTuple<string, int>));
            Assert.IsTrue(_sut.TypeValueTypleA.GetType() == typeof(ValueTuple<string, int>));
        }


        [Test]
        public void ValueTypeCheck()
        {
            Assert.IsFalse(_sut.SimpleTuple.GetType().IsValueType);
            Assert.IsTrue(_sut.SimpleValueTuple.GetType().IsValueType);
        }

        [Test]
        public void Deconstruction()
        {
            var (name, value) = _sut.SimpleTuple;
            Assert.AreEqual(name, "testTuple");
            Assert.AreEqual(value, 1);

            (name, value) = _sut.SimpleValueTuple;
            Assert.AreEqual(name, "testValueTuple");
            Assert.AreEqual(value, 2);
        }

        [Test]
        public void Deconstruction_AsIgnoreVariable()
        {
            var (name, _) = _sut.SimpleTuple;
            Assert.AreEqual(name, "testTuple");
            //Assert.AreEqual(_, 1); //Error CS0103  The name '_' does not exist in the current context CSharpValueTuple


            (name, _) = _sut.SimpleValueTuple;
            Assert.AreEqual(name, "testValueTuple");
            //Assert.AreEqual(_, 2); //Error CS0103  The name '_' does not exist in the current context CSharpValueTuple
        }
    }
}