using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity.Samples.QuadTree
{
    public interface ICoordinate
    {
        Vector2Int GetCoordinate();
    }

    public class Vector2IntQuadTree<T> where T : ICoordinate
    {
        public int Count { get; private set; }
        public Vector2IntQuadTree<T>[] Quadrants => _quadrants;
        public Vector2Int Pivot { get; private set; }
        public Vector2Int Min { get; private set; }
        public Vector2Int Max { get; private set; }

        private List<T> _objects;
        private Vector2IntQuadTree<T>[] _quadrants;
        private int _depth;
        private int _sqrMagnitude;

        public Vector2IntQuadTree(Vector2Int min, Vector2Int max)
        {
            _objects = new List<T>();
            Min = min;
            Max = max;
            Pivot = Min + Max / 2;
            _depth = 0;
            _sqrMagnitude = (Max - Min).sqrMagnitude;
            Debug.Log(_sqrMagnitude);
        }

        private Vector2IntQuadTree(Vector2Int min, Vector2Int max, int depth)
        {
            _objects = new List<T>();
            Min = min;
            Max = max;
            Pivot = (Min + Max) / 2;
            _depth = depth;
        }

        public void Add(T obj)
        {
            Vector2Int coord = obj.GetCoordinate();
            if (coord.x < Min.x || coord.x >= Max.x || coord.y < Min.y || coord.y >= Max.y)
            {
                throw new System.InvalidOperationException($"cannot add object to quadtree, {coord} is out of bounds");
            }

            Add(obj, coord);
        }
        private void Add(T obj, Vector2Int coord)
        {
            int index = GetQuadrantIndexFor(coord);
            if (index == -1)
            {
                _objects.Add(obj);
            }
            else
            {
                if (_quadrants == null)
                {
                    _quadrants = new Vector2IntQuadTree<T>[4];
                    _quadrants[0] = new Vector2IntQuadTree<T>(Pivot, Max, _depth + 1);
                    _quadrants[1] = new Vector2IntQuadTree<T>(new Vector2Int(Pivot.x, Min.y), new Vector2Int(Max.x, Pivot.y), _depth + 1);
                    _quadrants[2] = new Vector2IntQuadTree<T>(Min, Pivot, _depth + 1);
                    _quadrants[3] = new Vector2IntQuadTree<T>(new Vector2Int(Min.x, Pivot.y), new Vector2Int(Pivot.x, Max.y), _depth + 1);
                }
                _quadrants[index].Add(obj, coord);
            }
            Count++;
        }

        public bool Remove(T obj)
        {
            if (obj == null)
            {
                return false;
            }

            Vector2Int coord = obj.GetCoordinate();
            if (coord.x < Min.x || coord.x >= Max.x || coord.y < Min.y || coord.y >= Max.y)
            {
                throw new System.InvalidOperationException($"cannot remove object from quadtree, {coord} is out of bounds min: {Min}, max: {Max}, depth: {_depth}");
            }

            return Remove(obj, coord);
        }
        private bool Remove(T obj, Vector2Int coord)
        {
            int index = GetQuadrantIndexFor(coord);
            bool didRemove = false;
            if (index == -1)
            {
                didRemove |= _objects.Remove(obj);
            }
            else if (_quadrants != null)
            {
                didRemove |= _quadrants[index].Remove(obj, coord);
            }

            if (didRemove)
            {
                Count--;
            }

            if (Count <= 0)
            {
                _quadrants = null;
            }

            return didRemove;
        }

        public bool TryGet(Vector2Int coord, out List<T> toReturn)
        {
            int index = GetQuadrantIndexFor(coord);
            if (index == -1)
            {
                toReturn = _objects;
                return toReturn.Count > 0;
            }

            if (_quadrants == null)
            {
                toReturn = new List<T>();
                return false;
            }

            return _quadrants[index].TryGet(coord, out toReturn);
        }

        private int GetQuadrantIndexFor(Vector2Int coord)
        {
            Debug.Log(Pivot);
            if (_sqrMagnitude <= 1)
            {
                Debug.Log(coord + " return -1");
                return -1;
            }
            if (coord.x > Pivot.x)
            {
                if (coord.y > Pivot.y)
                {
                    Debug.Log(coord + " return 0");
                    return 0;
                }
                else
                {
                    Debug.Log(coord + " return 1");
                    return 1;
                }
            }
            else
            {
                if (coord.y < Pivot.y)
                {
                    Debug.Log(coord + " return 2");
                    return 2;
                }
                else
                {
                    Debug.Log(coord + " return 3");
                    return 3;
                }
            }
        }
    }
}