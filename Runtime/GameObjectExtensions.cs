using System;
using System.Linq;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class GameObjectExtensions
    {
        public static T RequireComponent<T>(this GameObject go) where T : Component
        {
            var toReturn = go.GetComponent<T>();
            if (toReturn == null)
                toReturn = go.AddComponent<T>();

            return toReturn;
        }

        public static T RequireType<T>(this GameObject go)
        {
            var toReturn = go.GetType<T>();
            if(toReturn == null)
                throw new InvalidOperationException($"the gameObject {go.name} must have a MonoBehaviour component of type {nameof(T)}");


            return toReturn;
        }

        public static T GetType<T>(this GameObject go)
        {
            return go.GetTypes<T>().FirstOrDefault();
        }

        public static T[] GetTypes<T>(this GameObject go)
        {
            return go.GetComponents<Component>().OfType<T>().ToArray();
        }

        public static TofuAnimator.Sequence Sequence(this GameObject go)
        {
            return new TofuAnimator.Sequence(go);
        }

        public static void PlaySequence(this GameObject go, TofuAnimator.Sequence sequence)
        {
            TofuAnimator.Play(go, sequence);
        }
    }
}