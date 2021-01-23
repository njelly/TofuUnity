using System.Collections;
using System.Security.Cryptography.X509Certificates;
using FixMath.NET;
using NUnit.Framework;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tests.Runtime
{
    public static class FixVector2Tests
    {
        [Test]
        public static void PropertyTests()
        {
            var rightTimesTwo = FixVector2.Right * new Fix64(2);
            Assert.IsTrue(rightTimesTwo.Magnitude == new Fix64(2));
            Assert.IsTrue(rightTimesTwo.SqrMagnitude == new Fix64(4));
            Assert.IsTrue(rightTimesTwo.Normalized.Equals(FixVector2.Right));
        }

        [Test]
        public static void OperatorTests()
        {
            var right = FixVector2.Right;
            var leftTimesTwo = FixVector2.Left * new Fix64(2);
            Assert.IsTrue(right.Equals(FixVector2.Right));
            Assert.IsTrue((right + leftTimesTwo).Equals(FixVector2.Left));
            Assert.IsTrue((right - leftTimesTwo).Equals(new FixVector2(new Fix64(3), Fix64.Zero)));
            Assert.IsTrue((right * new Fix64(2)).Equals(new FixVector2(new Fix64(2), Fix64.Zero)));;
            Assert.IsTrue((right / new Fix64(2)).Equals(new FixVector2(Fix64.One / new Fix64(2), Fix64.Zero)));
        }
    }
}