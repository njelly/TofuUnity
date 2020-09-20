using System;
using System.Linq;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class GameObjectExtensions
    {
        public static T RequireComponent<T>(this GameObject go) where T : MonoBehaviour
        {
            T toReturn = go.GetComponent<T>();
            if (toReturn == null)
            {
                toReturn = go.AddComponent<T>();
            }

            return toReturn;
        }

        public static T RequireInterface<T>(this GameObject go)
        {
            T toReturn = go.GetInterface<T>();
            if(toReturn == null)
            {
                throw new InvalidOperationException($"the gameObject {go.name} must have a MonoBehaviour component of type {nameof(T)}");
            }

            return toReturn;
        }

        public static T GetInterface<T>(this GameObject go)
        {
            return go.GetInterfaces<T>().FirstOrDefault();
        }

        public static T[] GetInterfaces<T>(this GameObject go)
        {
            if (!typeof(T).IsInterface)
            {
                throw new InvalidOperationException($"the type {nameof(T)} must be an interface");
            }

            return go.GetComponents<MonoBehaviour>().OfType<T>().ToArray();
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