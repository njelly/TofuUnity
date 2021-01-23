using System;
using FixMath.NET;
using Tofunaut.TofuECS;
using Tofunaut.TofuUnity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tests.Runtime.ECSExample
{
    public unsafe class ECSManager : MonoBehaviour
    {
        public ECSEntityView entityView;
        private ECS _ecs;

        private class ECSConfig
        {
            public Fix64 halfRange;
            public Fix64 deltaTime;
            public Fix64 g;
        }

        public struct Particle
        {
            public FixVector2 Position;
            public FixVector2 Velocity;
            public Fix64 Mass;
        }

        private class ParticleSystem : ECSSystem
        {
            public override void Update(Frame f)
            {
                var config = f.Config<ECSConfig>();
                
                var particleFilter = f.Filter<Particle>();
                while(particleFilter.Next(out var e, out var particle))
                    UpdateParticlePosition(f, e, particle, config);
                
                particleFilter.Reset();
                while (particleFilter.Next(out var e, out var particle))
                    UpdateParticleGravity(f, e, particle, config);
            }

            private static void UpdateParticlePosition(Frame f, ulong e, Particle* particle, ECSConfig config)
            {
                particle->Position += particle->Velocity * config.deltaTime;
                if (particle->Position.x > config.halfRange)
                    particle->Position = new FixVector2(particle->Position.x - config.halfRange * new Fix64(2),
                        particle->Position.y);
                if (particle->Position.x < -config.halfRange)
                    particle->Position = new FixVector2(particle->Position.x + config.halfRange * new Fix64(2),
                        particle->Position.y);
                if (particle->Position.y < -config.halfRange)
                    particle->Position = new FixVector2(particle->Position.x, 
                        particle->Position.y + config.halfRange * new Fix64(2));
                if (particle->Position.y > config.halfRange)
                    particle->Position = new FixVector2(particle->Position.x, 
                        particle->Position.y - config.halfRange * new Fix64(2));
            }

            private static void UpdateParticleGravity(Frame f, ulong e, Particle* particle, ECSConfig config)
            {
                particle->Position += particle->Velocity * config.deltaTime;

                var particleFilter = f.Filter<Particle>();
                while (particleFilter.Next(out var otherEntity, out var otherParticle))
                {
                    if (otherEntity == e)
                        continue;

                    var toOther = otherParticle->Position - particle->Position;
                    var sqrMagnitude = toOther.SqrMagnitude;
                    
                    if (sqrMagnitude <= Fix64.Zero) 
                        continue;

                    if (sqrMagnitude >= config.halfRange * config.halfRange)
                        continue;
                    
                    var forceGravity = (config.g * particle->Mass * otherParticle->Mass) / sqrMagnitude;
                    particle->Velocity += toOther.Normalized * forceGravity * config.deltaTime;
                }
            }
        }
        
        private void Awake()
        {
            var config = new ECSConfig
            {
                halfRange = new Fix64(40),
                deltaTime = (Fix64) 0.01667f,
                g = (Fix64) 6.6720e-08,
            };

            // because of the dumb physics, this will exponentially increase calculations
            const int maxParticles = 100;

            _ecs = new ECS(config);
            _ecs.CurrentFrame.RegisterComponent<Particle>(maxParticles);
            _ecs.CurrentFrame.AddSystem(new ParticleSystem());
            for (var i = 0; i < maxParticles; i++)
            {
                var particleEntity = _ecs.CurrentFrame.Create();
                _ecs.CurrentFrame.Add<Particle>(particleEntity);
            }

            var particleFilter = _ecs.CurrentFrame.Filter<Particle>();
            while (particleFilter.Next(out var e, out var particle))
            {
                particle->Mass = new Fix64(10000);
                particle->Position = new Vector2(Random.Range((float)-config.halfRange, (float)config.halfRange), Random.Range((float)-config.halfRange, (float)config.halfRange)).ToFixVector2();

                var ecsEntityView = Instantiate(entityView);
                ecsEntityView.transform.position = particle->Position.ToUnityVector2();
                ecsEntityView.Init(_ecs, e);
            }
        }

        private float _frameTimer;
        private void Update()
        {
            _frameTimer -= Time.deltaTime;
            if (_frameTimer > 0) 
                return;
            
            _frameTimer += (float)_ecs.CurrentFrame.Config<ECSConfig>().deltaTime;
            _ecs.Tick();
        }
    }
}