using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpWierd
{
    public class UnitTest2
    {
        [Fact]
        public void Test1()
        {

            var selections = Enumerable.Range(1, 10).Select(item => new Entity { });
            BatchProcess(selections);
        }

        public static void BatchProcess(IEnumerable<Entity> selections)
        {
            foreach (Entity selection in selections)
            {
                selection.Status = true;
                Console.WriteLine("Start Update : " + selection.Status);
            }
            BatchUpdate(selections);
        }

        public static void BatchUpdate(IEnumerable<Entity> selections)
        {
            foreach (Entity selection in selections)
            {
                Console.WriteLine(selection.Status);
                if (!selection.Status)
                    continue;
                Console.WriteLine("Start Update : " + selection.Status);
                Assert.False(selection.Status);
            }
        }

        public class Entity
        {
            public bool Status { get; set; }
        }
    }
}
