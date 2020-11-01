using System.Collections;
using NUnit.Framework;
using Tofunaut.TofuUnity;

namespace Tests.Runtime
{
    public static class FloatExtensionsTests
    {
        [Test]
        public static void FloatIsApproximatelyTest()
        {
            const float valueA = 1f;
            Assert.IsTrue(valueA.IsApproximately(1f));
            Assert.IsFalse(valueA.IsApproximately(2f));
            Assert.IsFalse(valueA.IsApproximately(0f));
            Assert.IsTrue(valueA.IsApproximately(2f, 1f));
            Assert.IsTrue(valueA.IsApproximately(0f, 1f));
        }
    }
}