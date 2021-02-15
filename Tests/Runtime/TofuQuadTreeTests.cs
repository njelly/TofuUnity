using NUnit.Framework;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tests.Runtime
{
    public static class TofuQuadTreeTests
    {
        private class Circle
        {
            public Vector2 position;
            public float radius;
        }
        
        [Test]
        public static void BasicTest()
        {
            var quadTree = new TofuQuadTree<Circle>(0, 0, 64, 64);

            // first some tests for the rect class
            var rectA = new TofuQuadTree<Circle>.Rect(0, 0, 4, 3);
            var rectB = new TofuQuadTree<Circle>.Rect(0, 5, 4, 3);
            var rectC = new TofuQuadTree<Circle>.Rect(2, 1, 1, 1);
            Assert.IsTrue(rectA.IsInBounds(rectC));
            Assert.IsFalse(rectC.IsInBounds(rectA));
            Assert.IsFalse(rectA.IsInBounds(rectB));

            var circleA = new Circle
            {
                position = new Vector2(4, 4),
                radius = 2f,
            };

            var circleB = new Circle
            {
                position = new Vector2(12, 3),
                radius = 4f,
            };

            var circleC = new Circle
            {
                position = new Vector2(49, 20),
                radius = 1f,
            };

            quadTree.Add(circleA, Mathf.RoundToInt(circleA.position.x), Mathf.RoundToInt(circleA.position.y),
                Mathf.RoundToInt(circleA.radius), Mathf.RoundToInt(circleA.radius));
            quadTree.Add(circleB, Mathf.RoundToInt(circleB.position.x), Mathf.RoundToInt(circleB.position.y),
                Mathf.RoundToInt(circleB.radius), Mathf.RoundToInt(circleB.radius));
            quadTree.Add(circleC, Mathf.RoundToInt(circleC.position.x), Mathf.RoundToInt(circleC.position.y),
                Mathf.RoundToInt(circleC.radius), Mathf.RoundToInt(circleC.radius));
            
            Assert.IsTrue(quadTree.Count == 3);
            
            // remove circleB and add it back
            quadTree.Remove(circleB, Mathf.RoundToInt(circleB.position.x), Mathf.RoundToInt(circleB.position.y),
                Mathf.RoundToInt(circleB.radius), Mathf.RoundToInt(circleB.radius));
            
            Assert.IsTrue(quadTree.Count == 2);
            
            circleB.position = new Vector2(3, 12);
            quadTree.Add(circleB, Mathf.RoundToInt(circleB.position.x), Mathf.RoundToInt(circleB.position.y),
                Mathf.RoundToInt(circleB.radius), Mathf.RoundToInt(circleB.radius));

            Assert.IsTrue(quadTree.Count == 3);

            var items = quadTree.Get(2, 2, 1, 1);
            Assert.IsTrue(items.Count == 1);
        }
    }
}