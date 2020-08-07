using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.Vector2IntPathfinder
{
    public class PathFinder : MonoBehaviour
    {
        public const int Size = 16;

        public Obstacle from;
        public Obstacle to;

        private Vector2Int _fromCoord;
        private Vector2Int _toCoord;

        private void OnEnable()
        {
            _fromCoord = Vector2Int.one * int.MinValue;
            _toCoord = Vector2Int.one * int.MinValue;
        }

        private void Update()
        {
            bool didChange = false;
            if (from)
            {
                Vector2Int fromCoord = from.Coord;
                if (fromCoord != _fromCoord)
                {
                    _fromCoord = fromCoord;
                    didChange = true;
                }
            }
            else
            {
                didChange = false;
            }
            if (to)
            {
                Vector2Int toCoord = to.Coord;
                if (toCoord != _toCoord)
                {
                    _toCoord = toCoord;
                    didChange = true;
                }
            }
            else
            {
                didChange = false;
            }

            if (didChange)
            {
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        OpenTile.SetAsPathMember(new Vector2Int(x, y), false);
                    }
                }

                Vector2Int[] path = TofuUnity.Vector2IntPathfinder.GetPath(_fromCoord, _toCoord, (Vector2Int coord) =>
                {
                    bool toReturn = coord.x >= 0 && coord.x < PathFinder.Size && coord.y >= 0 && coord.y < PathFinder.Size && Obstacle.CanOccupy(coord);
                    toReturn |= coord == _toCoord;
                    return toReturn;
                });

                for (int i = 0; i < path.Length; i++)
                {
                    OpenTile.SetAsPathMember(path[i], true);
                }
            }
        }
    }
}