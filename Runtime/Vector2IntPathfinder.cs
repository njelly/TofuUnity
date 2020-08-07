using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public static class Vector2IntPathfinder
    {
        private class Vector2IntAsAStarNode
        {
            public Vector2Int coord;
            public Vector2IntAsAStarNode previous;
            public int f, g, h;

            public Vector2Int[] ToPath()
            {
                List<Vector2Int> asList = new List<Vector2Int>();
                Vector2IntAsAStarNode current = this;
                while (current != null)
                {
                    asList.Add(current.coord);
                    current = current.previous;
                }
                asList.Reverse();
                return asList.ToArray();
            }
        }

        public delegate bool CanTravelDelegate(Vector2Int coord);
        public delegate int CalculateTravelCostDelegate(Vector2Int coord);

        private static bool DefaultCanTravelDelegate(Vector2Int coord) => false;
        private static int DefaultCalculateTravelCostDelegate(Vector2Int coord) => 0;

        public static Vector2Int[] GetPath(Vector2Int start, Vector2Int goal) => GetPath(start, goal, DefaultCanTravelDelegate, DefaultCalculateTravelCostDelegate, -1);
        public static Vector2Int[] GetPath(Vector2Int start, Vector2Int goal, CanTravelDelegate canTravelDelegate) => GetPath(start, goal, canTravelDelegate, DefaultCalculateTravelCostDelegate, -1);
        public static Vector2Int[] GetPath(Vector2Int start, Vector2Int goal, CalculateTravelCostDelegate calculateTravelCostDelegate) => GetPath(start, goal, DefaultCanTravelDelegate, calculateTravelCostDelegate, -1);
        public static Vector2Int[] GetPath(Vector2Int start, Vector2Int goal, CanTravelDelegate canTravelDelegate, int infiniteLoopLimit) => GetPath(start, goal, canTravelDelegate, DefaultCalculateTravelCostDelegate, infiniteLoopLimit);
        public static Vector2Int[] GetPath(Vector2Int start, Vector2Int goal, CalculateTravelCostDelegate calculateTravelCostDelegate, int infiniteLoopLimit) => GetPath(start, goal, DefaultCanTravelDelegate, calculateTravelCostDelegate, infiniteLoopLimit);
        public static Vector2Int[] GetPath(Vector2Int start, Vector2Int goal, CanTravelDelegate canTravelDelegate, CalculateTravelCostDelegate calculateTravelCostDelegate, int infiniteLoopLimit)
        {
            Vector2Int[] bestPath = new Vector2Int[0];

            HashSet<Vector2IntAsAStarNode> open = new HashSet<Vector2IntAsAStarNode>();
            HashSet<Vector2IntAsAStarNode> closed = new HashSet<Vector2IntAsAStarNode>();
            open.Add(new Vector2IntAsAStarNode
            {
                coord = start,
                previous = null,
                f = 0,
                g = 0,
                h = 0,
            });

            int counter = 0;
            while (open.Count > 0)
            {
                counter++;

                if (infiniteLoopLimit >= 0 && counter > infiniteLoopLimit)
                {
                    throw new Exception("Vector2IntPathFinder - infinite loop detected");
                }

                List<Vector2IntAsAStarNode> openAsList = new List<Vector2IntAsAStarNode>(open);
                openAsList.Sort((Vector2IntAsAStarNode a, Vector2IntAsAStarNode b) =>
                {
                    return a.f.CompareTo(b.f);
                });

                // set the current node to the node with the least f
                Vector2IntAsAStarNode currentNode = openAsList[0];

                closed.Add(openAsList[0]);
                open.Remove(currentNode);

                if (currentNode.coord.Equals(goal)) // remember to use .Equals() instead of == becuase these are not the same object
                {
                    bestPath = currentNode.ToPath();
                    break;
                }

                List<Vector2Int> potentialNextCoords = new List<Vector2Int>();
                if (canTravelDelegate(currentNode.coord + Vector2Int.up))
                {
                    potentialNextCoords.Add(currentNode.coord + Vector2Int.up);
                }
                if (canTravelDelegate(currentNode.coord + Vector2Int.down))
                {
                    potentialNextCoords.Add(currentNode.coord + Vector2Int.down);
                }
                if (canTravelDelegate(currentNode.coord + Vector2Int.left))
                {
                    potentialNextCoords.Add(currentNode.coord + Vector2Int.left);
                }
                if (canTravelDelegate(currentNode.coord + Vector2Int.right))
                {
                    potentialNextCoords.Add(currentNode.coord + Vector2Int.right);
                }

                foreach (Vector2Int coord in potentialNextCoords)
                {
                    Vector2IntAsAStarNode childNode = new Vector2IntAsAStarNode();
                    childNode.coord = coord;
                    childNode.previous = currentNode;
                    childNode.g = currentNode.g + 1;
                    childNode.h = (coord - goal).ManhattanDistance();
                    childNode.f = childNode.g + childNode.h + calculateTravelCostDelegate(coord);

                    // check if we've visited this coord but now we have a better path to it
                    bool alreadyVisited = false;
                    foreach (Vector2IntAsAStarNode openNode in open)
                    {
                        if (!openNode.coord.Equals(coord))
                        {
                            continue;
                        }

                        if (openNode.f <= childNode.f)
                        {
                            continue;
                        }

                        openNode.g = childNode.g;
                        openNode.h = childNode.h;
                        openNode.f = childNode.f;
                        openNode.previous = childNode.previous;
                        alreadyVisited = true;
                    }

                    foreach (Vector2IntAsAStarNode closedNode in closed)
                    {
                        if (closedNode.coord.Equals(coord))
                        {
                            alreadyVisited = true;
                            break;
                        }
                    }

                    if (!alreadyVisited)
                    {
                        open.Add(childNode);
                    }
                }
            }

            return bestPath;
        }

    }
}