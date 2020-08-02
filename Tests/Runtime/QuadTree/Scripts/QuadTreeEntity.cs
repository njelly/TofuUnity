using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.QuadTree
{
    public class QuadTreeEntity : MonoBehaviour, ICoordinate
    {
        private Vector2IntQuadTree<QuadTreeEntity> _quadTree;
        private Vector2Int _prevCoord = new Vector2Int(int.MinValue, int.MinValue);

        private void Update()
        {
            Vector2Int coord = GetCoordinate();
            if (coord != _prevCoord)
            {
                QuadTreeManager.Remove(this);
                QuadTreeManager.Add(this);
                _prevCoord = coord;
            }
        }

        public Vector2Int GetCoordinate()
        {
            return new Vector2Int(Mathf.FloorToInt(gameObject.transform.position.x), Mathf.FloorToInt(gameObject.transform.position.y));
        }
    }
}