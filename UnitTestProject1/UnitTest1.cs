using Mecalf.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = new[] { new MyClass(), new MyClass(), new MyClass(), new MyClass(), new MyClass(), new MyClass() }.ToList();
            list.Clear();
            var rs = list.Select(a => new MyClass() { A = a.A }).ToList();
            Console.WriteLine(rs.ToList().ToJson());
        }
    }

    class MyClass
    {
        public string A { get; set; }
        private static int a = 'a';

        public MyClass()
        {
            A = (a++).ToString();
        }
    }
}
