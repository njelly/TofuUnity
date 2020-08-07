using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.Vector2IntPathfinder
{
    public class Obstacle : MonoBehaviour
    {
        private static Vector2IntQuadTree<Obstacle> _quadTree = new Vector2IntQuadTree<Obstacle>(Vector2Int.zero, Vector2Int.one * PathFinder.Size);

        public Vector2Int Coord => new Vector2Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));

        public void OnEnable()
        {
            _quadTree.Add(this, Coord);
        }

        public void OnDisable()
        {
            _quadTree.Remove(this, Coord);
        }

        public static bool CanOccupy(Vector2Int coord)
        {
            if (_quadTree.TryGet(coord, out List<Obstacle> toReturn))
            {
                return toReturn.Count <= 0;
            }

            return true;
        }
    }
}