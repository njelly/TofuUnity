using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.Vector2IntPathfinder
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public Obstacle obstaclePrefab;
        public int obstacleSpawnTries;
        public OpenTile openTilePrefab;

        private List<Obstacle> _spawnedObstacles;
        private List<OpenTile> _spawnedOpenTiles;

        private void OnEnable()
        {
            Spawn();
        }

        public void Spawn()
        {
            if (_spawnedObstacles != null)
            {
                foreach (Obstacle obstacle in _spawnedObstacles)
                {
                    Destroy(obstacle.gameObject);
                }
                _spawnedObstacles.Clear();
            }
            else
            {
                _spawnedObstacles = new List<Obstacle>();
            }

            if (_spawnedOpenTiles != null)
            {
                foreach (OpenTile openTile in _spawnedOpenTiles)
                {
                    Destroy(openTile.gameObject);
                }
                _spawnedOpenTiles.Clear();
            }
            else
            {
                _spawnedOpenTiles = new List<OpenTile>();
            }

            for (int i = 0; i < obstacleSpawnTries; i++)
            {
                Vector2Int coord = new Vector2Int(Random.Range(0, PathFinder.Size), Random.Range(0, PathFinder.Size));
                if (!Obstacle.CanOccupy(coord))
                {
                    continue;
                }

                Obstacle obstacle = Instantiate(obstaclePrefab, new Vector3(coord.x, 0f, coord.y), Quaternion.identity);
                _spawnedObstacles.Add(obstacle);
            }

            for (int x = 0; x < PathFinder.Size; x++)
            {
                for (int y = 0; y < PathFinder.Size; y++)
                {
                    Vector2Int coord = new Vector2Int(x, y);
                    if (!Obstacle.CanOccupy(coord))
                    {
                        continue;
                    }

                    OpenTile openTile = Instantiate(openTilePrefab, new Vector3(coord.x, 0f, coord.y), Quaternion.identity);
                    _spawnedOpenTiles.Add(openTile);
                }
            }
        }
    }
}