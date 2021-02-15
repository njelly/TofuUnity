using System;
using System.Collections.Generic;

namespace Tofunaut.TofuUnity
{
    [Serializable]
    public class TofuQuadTree<T>
    {
        [Serializable]
        public class Rect
        {
            public int x, y, w, h;

            public Rect(int x, int y, int w, int h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }

            public bool IsInBounds(Rect other) => IsInBounds(other.x, other.y, other.w, other.h);

            public bool IsInBounds(int ox, int oy, int ow, int oh)
            {
                return ox >= x && ox < x + w && ox + ow >= x && ox + ow < x + w
                       && oy >= y && oy < y + h && oy + oh >= y && oy + oh < y + h;
            }
        }

        public int Count => _count;
        
        private Rect _rect;
        private TofuQuadTree<T>[] _children;
        private List<T> _items;
        private int _count;

        public TofuQuadTree(int x, int y, int w, int h)
        {
            _rect = new Rect(x, y, w, h);
            _items = new List<T>();
        }

        private TofuQuadTree(Rect rect)
        {
            _rect = rect;
            _items = new List<T>();
        }

        public void Add(T item, int x, int y, int w, int h)
        {
            _count++;
            
            if (_rect.w == 1 || _rect.h == 1)
            {
                // this is a leaf node, we could not possibly subdivide further, so add and return
                _items.Add(item);
                return;
            }

            // create the squares first and check them, so we don't create children unnecessarily 
            var childSquares = new Rect[4];
            var halfWidth = _rect.w / 2;
            var halfHeight = _rect.h / 2;
            childSquares[0] = new Rect(_rect.x + halfWidth, _rect.y + halfHeight, halfWidth, halfHeight); // top right
            childSquares[1] = new Rect(_rect.x + halfWidth, _rect.y, halfWidth, halfHeight); // bottom right
            childSquares[2] = new Rect(_rect.x, _rect.y, halfWidth, halfHeight); // bottom left
            childSquares[3] = new Rect(_rect.x, _rect.y + halfHeight, halfWidth, halfHeight); // top left
            var childIndex = -1;
            for (var i = 0; i < childSquares.Length; i++)
            {
                if (!childSquares[i].IsInBounds(x, y, w, h)) 
                    continue;
                
                childIndex = i;
                break;
            }
            
            if(childIndex == -1)
                _items.Add(item); // this item doesn't fit neatly into any child, so keep it on this node
            else
            {
                if (_children == null)
                {
                    _children = new TofuQuadTree<T>[4];
                    _children[0] = new TofuQuadTree<T>(childSquares[0]);
                    _children[1] = new TofuQuadTree<T>(childSquares[1]);
                    _children[2] = new TofuQuadTree<T>(childSquares[2]);
                    _children[3] = new TofuQuadTree<T>(childSquares[3]);
                }
                
                _children[childIndex].Add(item, x, y, w, h);
            }
        }

        public bool Remove(T item, int x, int y, int w, int h)
        {
            if (!_rect.IsInBounds(x, y, w, h))
                return false;
            
            var childIndex = -1;

            if (_children != null)
            {
                for (var i = 0; i < _children.Length; i++)
                {
                    if (!_children[i].Remove(item, x, y, w, h)) 
                        continue;
                
                    childIndex = i;
                    break;
                }
            }

            var didRemove = childIndex != -1;

            if (didRemove)
            {
                // if we removed an item from our children, and the count of our list accounts for the entire count of
                // our tree, then remove our children (because they must be empty). This keeps the size of the object
                // small and prunes any unused branches.
                if(_count == _items.Count + 1) // +1 because we haven't subtracted from our count yet
                    _children = null;
            }
            else
                didRemove = _items.Remove(item);

            if (didRemove)
                _count--;

            return didRemove;
        }

        public List<T> Get(int x, int y, int w, int h) => Get(new Rect(x, y, w, h));
        private List<T> Get(Rect r)
        {
            var toReturn = new List<T>();
            
            if (!_rect.IsInBounds(r) || _children == null) 
                return toReturn;
            
            toReturn.AddRange(_items);
            
            foreach (var child in _children)
                toReturn.AddRange(child.Get(r));

            return toReturn;
        }
    }
}