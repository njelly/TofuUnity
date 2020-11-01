using System.Collections;
using NUnit.Framework;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class InputValueTests
    {
        [UnityTest]
        public IEnumerator InputButtonTestCoroutine()
        {
            var button = new InputButton();
            
            Assert.IsFalse(button.WasPressed);
            Assert.IsFalse(button.WasReleased);
            Assert.IsFalse(button.Held);
            
            button.Press();
            
            Assert.IsTrue(button.WasPressed);
            Assert.IsFalse(button.WasReleased);
            Assert.IsTrue(button.Held);

            yield return null;
            
            Assert.IsFalse(button.WasPressed);
            Assert.IsFalse(button.WasReleased);
            Assert.IsTrue(button.Held);
            
            button.Release();
            
            Assert.IsFalse(button.WasPressed);
            Assert.IsTrue(button.WasReleased);
            Assert.IsFalse(button.Held);

            yield return null;
            
            Assert.IsFalse(button.WasPressed);
            Assert.IsFalse(button.WasReleased);
            Assert.IsFalse(button.Held);
        }

        [UnityTest]
        public IEnumerator InputSingleAxisTestCoroutine()
        {
            var singleAxis = new InputSingleAxis();
            
            Assert.IsFalse(singleAxis.WasPressed);
            Assert.IsFalse(singleAxis.WasReleased);
            Assert.IsFalse(singleAxis.Held);
            
            singleAxis.SetAxis(0f);
            
            Assert.IsFalse(singleAxis.WasPressed);
            Assert.IsFalse(singleAxis.WasReleased);
            Assert.IsFalse(singleAxis.Held);
            
            singleAxis.SetAxis(0.5f);
            
            Assert.IsTrue(singleAxis.WasPressed);
            Assert.IsFalse(singleAxis.WasReleased);
            Assert.IsTrue(singleAxis.Held);

            yield return null;
            
            Assert.IsFalse(singleAxis.WasPressed);
            Assert.IsFalse(singleAxis.WasReleased);
            Assert.IsTrue(singleAxis.Held);
            
            singleAxis.SetAxis(0f);
            
            Assert.IsFalse(singleAxis.WasPressed);
            Assert.IsTrue(singleAxis.WasReleased);
            Assert.IsFalse(singleAxis.Held);

            yield return null;
            
            Assert.IsFalse(singleAxis.WasPressed);
            Assert.IsFalse(singleAxis.WasReleased);
            Assert.IsFalse(singleAxis.Held);
        }

        [UnityTest]
        public IEnumerator InputDoubleAxisTestCoroutine()
        {
            var doubleAxis = new InputDoubleAxis();
            
            Assert.IsFalse(doubleAxis.WasPressed);
            Assert.IsFalse(doubleAxis.WasReleased);
            Assert.IsFalse(doubleAxis.Held);
            
            doubleAxis.SetAxis(Vector2.zero);
            
            Assert.IsFalse(doubleAxis.WasPressed);
            Assert.IsFalse(doubleAxis.WasReleased);
            Assert.IsFalse(doubleAxis.Held);
            
            doubleAxis.SetAxis(Vector2.right);
            
            Assert.IsTrue(doubleAxis.WasPressed);
            Assert.IsFalse(doubleAxis.WasReleased);
            Assert.IsTrue(doubleAxis.Held);

            yield return null;
            
            Assert.IsFalse(doubleAxis.WasPressed);
            Assert.IsFalse(doubleAxis.WasReleased);
            Assert.IsTrue(doubleAxis.Held);
            
            doubleAxis.SetAxis(Vector2.zero);
            
            Assert.IsFalse(doubleAxis.WasPressed);
            Assert.IsTrue(doubleAxis.WasReleased);
            Assert.IsFalse(doubleAxis.Held);

            yield return null;
            
            Assert.IsFalse(doubleAxis.WasPressed);
            Assert.IsFalse(doubleAxis.WasReleased);
            Assert.IsFalse(doubleAxis.Held);
        }
    }
}