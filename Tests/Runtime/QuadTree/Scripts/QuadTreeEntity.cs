using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.QuadTree
{
    [ExecuteInEditMode]
    public class QuadTreeEntity : MonoBehaviour
    {
        private Vector2Int _prevCoord = new Vector2Int(int.MinValue, int.MinValue);

        private void Update()
        {
            Vector2Int coord = GetCoordinate();
            if (coord != _prevCoord)
            {
                QuadTreeManager.Remove(this, _prevCoord);
                QuadTreeManager.Add(this);

                _prevCoord = coord;
            }
        }

        public Vector2Int GetCoordinate()
        {
            return new Vector2Int(Mathf.CeilToInt(gameObject.transform.position.x), Mathf.CeilToInt(gameObject.transform.position.y));
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.Label(transform.position, $"{_prevCoord}");
        }
#endif
    }
}