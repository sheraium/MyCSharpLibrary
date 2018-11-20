using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MyCSharpLibrary.UnitTests
{
    [TestClass]
    public class TSPTests
    {
        [TestMethod]
        public void OneCrane2Command()
        {
            var values = new[] { "1", "2", "3", "4", "5", "6" };

            var data = Permutation(values);

            var craneStartCost = new Dictionary<string, int>();
            craneStartCost.Add("A", 2);
            craneStartCost.Add("B", 1);

            var otherCost = new Dictionary<string, Dictionary<string, int>>();
            var aCost = new Dictionary<string, int>();
            aCost.Add("B", 2);
            otherCost.Add("A", aCost);

            var bCost = new Dictionary<string, int>();
            bCost.Add("A", 4);
            otherCost.Add("B", bCost);

            
        }

        private List<List<T>> Permutation<T>(IEnumerable<T> values)
        {
            var data = new List<List<T>>();
            permute(new List<T>(), new List<T>(values));
            return data;

            void permute(List<T> result, List<T> now)
            {
                if (now.Count == 0)
                {
                    data.Add(new List<T>(result));
                }
                else
                {
                    for (int i = 0; i < now.Count; i++)
                    {
                        var newResult = new List<T>();
                        newResult.AddRange(result);
                        newResult.Add(now[i]);
                        var newNow = now.GetRange(0, i);
                        newNow.AddRange(now.GetRange(i + 1, now.Count - (i + 1)));
                        permute(newResult, newNow);
                    }
                }
            }
        }
    }
}