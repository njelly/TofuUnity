using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class BallSpawner : MonoBehaviour
    {
        public GameObject ballPrefab;
        public float rangeRadius;

        public void Spawn()
        {
            float rotation = Random.Range(0f, Mathf.PI * 2f);
            Vector3 offset = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * Random.Range(0f, rangeRadius);
            Instantiate(ballPrefab, transform.position + offset, Quaternion.identity);
        }
    }
}