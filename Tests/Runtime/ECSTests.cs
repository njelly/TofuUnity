using System;
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
            var data = new Config
            {
                DeltaTime = 0.01667f,
            };

            var ecs = new ECS(data);
            var e = ecs.CurrentFrame.Create();
            ecs.CurrentFrame.AddSystem(new CountSystem());
            ecs.CurrentFrame.AddSystem(new MovementSystem());
            ecs.CurrentFrame.RegisterComponent<Count>(1024);
            ecs.CurrentFrame.RegisterComponent<TransformVel>(1024);
            ecs.CurrentFrame.Add<Count>(e);
            var mover = ecs.CurrentFrame.Create();
            ecs.CurrentFrame.Add<TransformVel>(mover);
            var m = ecs.CurrentFrame.Get<TransformVel>(mover);
            m->position = Vector2.zero;
            m->velocity = new Vector2(0.3451f, -0.93f);

            // create an entity, CountSystem should count up to num value
            const int num = 38;
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

            var movingTransform = ecs.CurrentFrame.Get<TransformVel>(mover);
            Debug.Log(movingTransform->position.ToString("F3"));
        }

        [Serializable]
        private class Config
        {
            public float DeltaTime;
        }

        private struct Count
        {
            public int value;
        }

        private struct TransformVel
        {
            public Vector2 position;
            public Vector2 velocity;
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
        
        private class MovementSystem : ECSSystem
        {
            public override void Update(Frame f)
            {
                var filter = f.Filter<TransformVel>();
                while (filter.Next(out _, out var transformVel))
                    transformVel->position += transformVel->velocity * f.Config<Config>().DeltaTime;
            }
        }
    }
}