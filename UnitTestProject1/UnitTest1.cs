using System;
using System.Linq;
using Mecalf.Common.Utility;
using Mecalf.Web.Framework.ClientBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [TestMethod]
        public void MyTestMethod()
        {
            Console.WriteLine($"{DateTime.Now:yyyy/MM-dd}");
        }
        [TestMethod]
        public void MyTestMethod2()
        {

        }
    }

    class MyClass
    {
        public string A { get; set; }
        private static int _a = 'a';

        /// <summary>
        /// 默认构造方法
        /// </summary>
        public MyClass()
        {
            A = (_a++).ToString();
        }
    }
}
