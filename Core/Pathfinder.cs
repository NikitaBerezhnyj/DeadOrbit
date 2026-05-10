using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
{
    public static class Pathfinder
    {
        private class Node
        {
            public GridPosition Pos;
            public Node Parent;
            public float G;
            public float H;
            public float F => G + H;
        }

        private static readonly (int dx, int dy, float cost)[] Neighbors =
        {
            (0, -1, 1f),
            (0, 1, 1f),
            (-1, 0, 1f),
            (1, 0, 1f),
            (-1, -1, 1.41f),
            (1, -1, 1.41f),
            (-1, 1, 1.41f),
            (1, 1, 1.41f),
        };

        public static List<GridPosition> FindPath(
            GridPosition start,
            GridPosition goal,
            HashSet<GridPosition> blocked
        )
        {
            var open = new List<Node>();
            var closed = new HashSet<GridPosition>();

            open.Add(
                new Node
                {
                    Pos = start,
                    G = 0,
                    H = Heuristic(start, goal),
                }
            );

            while (open.Count > 0)
            {
                var current = open[0];
                for (int i = 1; i < open.Count; i++)
                    if (open[i].F < current.F)
                        current = open[i];

                if (current.Pos == goal)
                    return BuildPath(current);

                open.Remove(current);
                closed.Add(current.Pos);

                foreach (var (dx, dy, cost) in Neighbors)
                {
                    var neighborPos = new GridPosition(current.Pos.X + dx, current.Pos.Y + dy);

                    if (closed.Contains(neighborPos))
                        continue;
                    if (blocked.Contains(neighborPos))
                        continue;

                    if (
                        neighborPos.X < 0
                        || neighborPos.Y < 0
                        || neighborPos.X >= TileGrid.WorldW
                        || neighborPos.Y >= TileGrid.WorldH
                    )
                        continue;

                    float g = current.G + cost;
                    var existing = open.Find(n => n.Pos == neighborPos);

                    if (existing == null)
                    {
                        open.Add(
                            new Node
                            {
                                Pos = neighborPos,
                                Parent = current,
                                G = g,
                                H = Heuristic(neighborPos, goal),
                            }
                        );
                    }
                    else if (g < existing.G)
                    {
                        existing.G = g;
                        existing.Parent = current;
                    }
                }
            }

            return null;
        }

        private static float Heuristic(GridPosition a, GridPosition b)
        {
            float dx = System.Math.Abs(a.X - b.X);
            float dy = System.Math.Abs(a.Y - b.Y);
            return dx + dy + (1.41f - 2f) * System.Math.Min(dx, dy);
        }

        private static List<GridPosition> BuildPath(Node node)
        {
            var path = new List<GridPosition>();
            while (node != null)
            {
                path.Add(node.Pos);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}
