using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MyCSharpLibrary.UnitTests
{
    [TestClass]
    public class ShortestPathTests
    {
        [TestMethod]
        public void ValueTypeNode()
        {
            var shorestPath = new ShortestPath<int>();

            var map = new Dictionary<int, List<int>>();
            map.Add(1, new List<int>() { 2, 3 });
            map.Add(2, new List<int>() { 1, 4, 5 });
            map.Add(3, new List<int>() { 1, 4 });
            map.Add(4, new List<int>() { 2, 3, 5 });
            map.Add(5, new List<int>() { 2, 4 });

            foreach (var A in map)
            {
                foreach (var B in A.Value)
                {
                    shorestPath.AddPath(A.Key, B);
                    shorestPath.SetCost(A.Key, B, 1);
                }
            }

            var actual = shorestPath.GetPath(1, 5);
            var expected = new List<int>() { 1, 2, 5 };
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void ReferenceTypeNode()
        {
            var shorestPath = new ShortestPath<string>();

            var map = new Dictionary<string, List<string>>();
            map.Add("1", new List<string>() { "2", "3" });
            map.Add("2", new List<string>() { "1", "4", "5" });
            map.Add("3", new List<string>() { "1", "4" });
            map.Add("4", new List<string>() { "2", "3", "5" });
            map.Add("5", new List<string>() { "2", "4" });

            foreach (var A in map)
            {
                foreach (var B in A.Value)
                {
                    shorestPath.AddPath(A.Key, B);
                    shorestPath.SetCost(A.Key, B, 1);
                }
            }

            var actual = shorestPath.GetPath("1", "5");
            var expected = new List<string>() { "1", "2", "5" };
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}