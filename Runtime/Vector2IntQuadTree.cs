using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class Vector2IntQuadTree<T>
    {
        public delegate Vector2Int GetVector2IntFrom(T obj);

        public int Count { get; private set; }
        public Vector2IntQuadTree<T>[] Quadrants { get; private set; }
        public Vector2Int Pivot { get; private set; }
        public Vector2Int Min { get; private set; }
        public Vector2Int Max { get; private set; }
        public int Depth { get; private set; }

        private GetVector2IntFrom _getFrom;
        private List<T> _objects;

        public Vector2IntQuadTree(Vector2Int min, Vector2Int max, GetVector2IntFrom getFrom)
        {
            _getFrom = getFrom;
            _objects = new List<T>();
            Min = min;
            Max = max;

            Pivot = (Min + Max) / 2;
            if (Math.Abs(Min.x - Max.x) <= 1)
            {
                Pivot = new Vector2Int(Min.x, Pivot.y);
            }
            if (Math.Abs(Min.y - Max.y) <= 1)
            {
                Pivot = new Vector2Int(Pivot.x, Min.y);
            }

            Depth = 0;
        }

        private Vector2IntQuadTree(Vector2Int min, Vector2Int max, int depth)
        {
            _objects = new List<T>();
            Min = min;
            Max = max;

            Pivot = (Min + Max) / 2;
            if (Math.Abs(Min.x - Max.x) <= 1)
            {
                Pivot = new Vector2Int(Min.x, Pivot.y);
            }
            if (Math.Abs(Min.y - Max.y) <= 1)
            {
                Pivot = new Vector2Int(Pivot.x, Min.y);
            }

            Depth = depth;
        }

        public void Add(T obj)
        {
            Vector2Int coord = _getFrom(obj);
            if (coord.x <= Min.x || coord.x > Max.x || coord.y <= Min.y || coord.y > Max.y)
            {
                throw new System.InvalidOperationException($"cannot remove object from quadtree, {coord} is out of bounds min: {Min}, max: {Max}, depth: {Depth}");
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
                if (Quadrants == null)
                {
                    Quadrants = new Vector2IntQuadTree<T>[]
                    {
                        new Vector2IntQuadTree<T>(Pivot, Max, Depth + 1),
                        new Vector2IntQuadTree<T>(new Vector2Int(Pivot.x, Min.y), new Vector2Int(Max.x, Pivot.y), Depth + 1),
                        new Vector2IntQuadTree<T>(Min, Pivot, Depth + 1),
                        new Vector2IntQuadTree<T>(new Vector2Int(Min.x, Pivot.y), new Vector2Int(Pivot.x, Max.y), Depth + 1),
                    };
                }
                Quadrants[index].Add(obj, coord);
            }
            Count++;
        }

        public bool Remove(T obj)
        {
            if (obj == null)
            {
                return false;
            }

            Vector2Int coord = _getFrom(obj);
            if (coord.x <= Min.x || coord.x > Max.x || coord.y <= Min.y || coord.y > Max.y)
            {
                throw new System.InvalidOperationException($"cannot remove object from quadtree, {coord} is out of bounds min: {Min}, max: {Max}, depth: {Depth}");
            }

            return Remove(obj, coord);
        }
        public bool Remove(T obj, Vector2Int coord)
        {
            int index = GetQuadrantIndexFor(coord);
            bool didRemove = false;
            if (index == -1)
            {
                didRemove |= _objects.Remove(obj);
            }
            else if (Quadrants != null)
            {
                didRemove |= Quadrants[index].Remove(obj, coord);
            }

            if (didRemove)
            {
                Count--;
            }

            if (Count <= 0)
            {
                Quadrants = null;
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

            if (Quadrants == null)
            {
                toReturn = null;
                return false;
            }

            return Quadrants[index].TryGet(coord, out toReturn);
        }

        public void Clear()
        {
            _objects.Clear();
            Quadrants = null;
        }

        private int GetQuadrantIndexFor(Vector2Int coord)
        {
            if (Pivot == Min)
            {
                return -1;
            }
            if (coord.x > Pivot.x)
            {
                if (coord.y > Pivot.y)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (coord.y > Pivot.y)
                {
                    return 3;
                }
                else
                {
                    return 2;
                }
            }
        }
    }
}