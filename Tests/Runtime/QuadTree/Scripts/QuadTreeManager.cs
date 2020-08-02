using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.QuadTree
{
    public class QuadTreeManager : SingletonBehaviour<QuadTreeManager>
    {
        private Vector2IntQuadTree<QuadTreeEntity> _quadTree;

        protected override void Awake()
        {
            base.Awake();

            _quadTree = new Vector2IntQuadTree<QuadTreeEntity>(Vector2Int.one * -64, Vector2Int.one * 64);
        }

        private void Update()
        {
            Vector2Int v = new Vector2Int(1, 1);
            Debug.Log($"layermask at {v} is {GetLayerMaskAt(v)}");
        }

        private void OnDrawGizmos()
        {
            if (_quadTree != null)
            {
                RenderQuadTree(_quadTree);
            }
        }

        private void RenderQuadTree<T>(Vector2IntQuadTree<T> tree) where T : ICoordinate
        {
            Gizmos.DrawLine(new Vector2(tree.Min.x, tree.Min.y), new Vector2(tree.Min.x, tree.Max.y));
            Gizmos.DrawLine(new Vector2(tree.Min.x, tree.Max.y), new Vector2(tree.Max.x, tree.Max.y));

            if (tree.Quadrants != null)
            {
                for (int i = 0; i < tree.Quadrants.Length; i++)
                {
                    if (tree.Quadrants[i] != null)
                    {
                        RenderQuadTree(tree.Quadrants[i]);
                    }
                }
            }
        }

        public static void Add(QuadTreeEntity e)
        {
            _instance._quadTree.Add(e);
        }

        public static bool Remove(QuadTreeEntity e)
        {
            return _instance._quadTree.Remove(e);
        }

        public int GetLayerMaskAt(Vector2Int coord)
        {
            _instance._quadTree.TryGet(coord, out List<QuadTreeEntity> atCoord);
            int mask = 0;
            foreach (QuadTreeEntity e in atCoord)
            {
                mask |= e.gameObject.layer;
            }

            return mask;
        }
    }
}