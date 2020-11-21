using NUnit.Framework;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tests.Runtime
{
    public class MathfUtilsTests
    {
        [Test]
        public void TestSmallestAngleDiff()
        {
            const float fromDeg = 350f;
            const float toDeg = 10f;
            
            Assert.IsTrue(MathfUtils.SmallestAngleDifferenceDeg(fromDeg, toDeg).IsApproximately(20f));

            const float fromRad = fromDeg * Mathf.Deg2Rad;
            const float toRad = toDeg * Mathf.Deg2Rad;
            
            Assert.IsTrue(MathfUtils.SmallestAngleDifferenceRad(fromRad, toRad).IsApproximately(20f * Mathf.Deg2Rad));
        }
    }
}