using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.Vector2IntPathfinder
{
    public class OpenTile : MonoBehaviour
    {
        private static Vector2IntQuadTree<OpenTile> _quadTree = new Vector2IntQuadTree<OpenTile>(Vector2Int.zero, Vector2Int.one * PathFinder.Size);

        public Vector2Int Coord => new Vector2Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));

        [SerializeField] private MeshRenderer _meshRenderer;

        public void OnEnable()
        {
            _quadTree.Add(this, Coord);
        }

        public void OnDisable()
        {
            _quadTree.Remove(this, Coord);
        }

        public static void SetAsPathMember(Vector2Int coord, bool isPathMember)
        {
            if (_quadTree.TryGet(coord, out List<OpenTile> tiles))
            {
                Color color = isPathMember ? Color.green : Color.white;
                foreach (OpenTile tile in tiles)
                {
                    tile._meshRenderer.material.color = color;
                }
            }
        }
    }
}