using System;
using Tofunaut.TofuECS;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tests.Runtime.ECSExample
{
    public unsafe class ECSEntityView : MonoBehaviour
    {
        public ulong Entity { get; private set; }

        private ECS _ecs;

        public void Init(ECS ecs, ulong entity)
        {
            _ecs = ecs;
            Entity = entity;
        }

        public void Update()
        {
            if (_ecs == null)
                return;

            var particle = _ecs.CurrentFrame.Get<ECSManager.Particle>(Entity);
            transform.position = particle->Position.ToUnityVector2();
        }
    }
}