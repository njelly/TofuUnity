using NUnit.Framework;
using Tofunaut.TofuECS;
using UnityEngine;

namespace Tests.Runtime
{
    public static unsafe class ECSTests
    {
        [Test]
        public static void CountTest()
        {
            var ecs = new ECS();
            var e = ecs.CurrentFrame.Create();
            ecs.CurrentFrame.AddSystem(new CountSystem());
            ecs.CurrentFrame.RegisterComponent<Count>(1024);
            ecs.CurrentFrame.Add<Count>(e);

            // create an entity, CountSystem should count up to num value
            const int num = 10;
            for(var i = 0; i < num; i++)
                ecs.Tick();

            Assert.IsTrue(ecs.CurrentFrame.Get<Count>(e)->value == num);
            
            // destroy the entity, CountSystem should no longer count up for e
            ecs.CurrentFrame.Destroy(e);
            
            for(var i = 0; i < num; i++)
                ecs.Tick();
            
            Assert.IsTrue(ecs.CurrentFrame.Get<Count>(e) == null);

            // get a new entity, create a dummy first 
            ecs.CurrentFrame.Create();
            e = ecs.CurrentFrame.Create();
            ecs.CurrentFrame.Add<Count>(e);
            for(var i = 0; i < num; i++)
                ecs.Tick();
            
            Assert.IsTrue(ecs.CurrentFrame.Get<Count>(e)->value == num);
        }

        private struct Count
        {
            public int value;
        }

        private class CountSystem : ECSSystem
        {
            public override void Update(Frame f)
            {
                var filter = f.Filter<Count>();
                while (filter.Next(out var e, out var count))
                    count->value++;
            }
        }
    }
}