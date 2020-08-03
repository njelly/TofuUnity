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

            _quadTree = new Vector2IntQuadTree<QuadTreeEntity>(Vector2Int.one * -512, Vector2Int.one * 512);
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

        private void RenderQuadTree<T>(Vector2IntQuadTree<T> tree)
        {
            Debug.DrawLine(new Vector2(tree.Min.x, tree.Max.y), new Vector2(tree.Max.x, tree.Max.y), tree.Depth % 2 == 0 ? Color.red : Color.green);
            Debug.DrawLine(new Vector2(tree.Max.x, tree.Max.y), new Vector2(tree.Max.x, tree.Min.y), tree.Depth % 2 == 0 ? Color.red : Color.green);

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

        public static void Add(QuadTreeEntity e, Vector2Int coord)
        {
            if (!_instance)
            {
                return;
            }

            _instance._quadTree.Add(e, coord);
        }

        public static bool Remove(QuadTreeEntity e, Vector2Int coord)
        {
            if (!_instance)
            {
                return false;
            }

            return _instance._quadTree.Remove(e, coord);
        }

        public static void Translate(QuadTreeEntity e, Vector2Int from, Vector2Int to)
        {
            if (!_instance)
            {
                return;
            }

            _instance._quadTree.Translate(e, from, to);
        }

        public int GetLayerMaskAt(Vector2Int coord)
        {
            int mask = 0;

            if (_instance._quadTree.TryGet(coord, out List<QuadTreeEntity> atCoord))
            {
                foreach (QuadTreeEntity e in atCoord)
                {
                    mask |= e.gameObject.layer;
                }
            }

            return mask;
        }
    }
}