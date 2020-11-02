using NUnit.Framework;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;

namespace Tests.Runtime
{
    public static class BlackboardTests
    {
        private class TestEvent : IBlackboardEvent
        {
            public readonly string message;

            public TestEvent(string message)
            {
                this.message = message;
            }
        }
        
        [Test]
        public static void SubscribeUnsubscribeTest()
        {
            var bb = new Blackboard();
            var numCallbackCalls = 0;

            void Callback(TestEvent testEvent)
            {
                numCallbackCalls++;
            }
            
            bb.Invoke(new TestEvent("hello world"));
            bb.Subscribe<TestEvent>(Callback);
            for (var i = 0; i < 10; i++)
                bb.Invoke(new TestEvent("hello world"));
            
            Assert.IsTrue(numCallbackCalls == 10);
            
            bb.Unsubscribe<TestEvent>(Callback);
            for (var i = 0; i < 10; i++)
                bb.Invoke(new TestEvent("hello world"));
            
            Assert.IsTrue(numCallbackCalls == 10);
        }
    }
}