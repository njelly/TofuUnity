using System;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class GameObjectExtentions
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