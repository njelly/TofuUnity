using System.Collections;
using NUnit.Framework;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class TofuAnimatorTests
    {
        [UnityTest]
        public IEnumerator CountTest()
        {
            // count ensures callbacks are invoke even with very small intervals
            // 16777216 appears to be the max within 1 second on a 2018 mac mini (start: 0, end:  16777216, countTime: 1s)
            const int start = 0;
            const int end = 16777216; 
            const float countTime = 1f;
            
            var gameObject = new GameObject("CountTest");
            var count = start;
            var timesCounted = 0;
            gameObject.Sequence().Count(start, end, countTime / Mathf.Abs(end - start), i =>
            {
                timesCounted++;
                count = i;
            }).Play();
            
            yield return new WaitForSeconds(countTime);
            
            Assert.IsTrue(timesCounted == end - start);
            Assert.IsTrue(count == end);
        }
    }
}