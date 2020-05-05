using System;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class GameObjectExtentions
    {
        public static MonoBehaviour RequireComponent<T>(this GameObject go) where T : MonoBehaviour
        {
            T toReturn = go.GetComponent<T>();
            if (toReturn == null)
            {
                toReturn = go.AddComponent<T>();
            }

            return toReturn;
        }
    }
}