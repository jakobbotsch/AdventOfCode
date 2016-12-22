using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCSharp
{
    internal static class Day22
    {
        internal static void Solve(string input)
        {
            Dictionary<(int, int), Node> nodes = new Dictionary<(int, int), Node>();
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None).Skip(2).ToArray();
            foreach (string line in lines)
            {
                var match = Regex.Match(line, "/dev/grid/node-x(\\d+)-y(\\d+)\\s+(\\d+)T\\s+(\\d+)T\\s+(\\d+)T");
                int x = int.Parse(match.Groups[1].Value);
                int y = int.Parse(match.Groups[2].Value);
                int used = int.Parse(match.Groups[4].Value);
                int avail = int.Parse(match.Groups[5].Value);

                nodes.Add((x, y), new Node(x, y, used + avail) { Used = used });
            }

            int pairs = 0;
            foreach (var node in nodes.Values)
            {
                foreach (var node2 in nodes.Values)
                {
                    if (node.Used == 0 || node2 == node)
                        continue;

                    if (node.Used < node2.Avail)
                        pairs++;
                }
            }

            Console.WriteLine("Pairs: {0}", pairs);

            Node free = nodes[(35, 27)];
            Node moving = nodes[(35, 0)];
            int moves = 0;
            while (moving.X != 0 || moving.Y != 0)
            {
                moves += Move(free.X, free.Y, moving.X - 1, moving.Y);
                free = nodes[(moving.X - 1, moving.Y)];
                free.Used += moving.Used;
                moving.Used = 0;

                var temp = moving;
                moving = free;
                free = temp;
                moves++;
            }

            Console.WriteLine("Moves: {0}", moves);

            int Move(int sx, int sy, int ex, int ey)
            {
                Queue<Node> queue = new Queue<Node>();
                foreach (var node in nodes.Values)
                    node.Prev = null;

                queue.Enqueue(nodes[(sx, sy)]);
                queue.Peek().Prev = queue.Peek();
                (int, int)[] nos = { (-1, 0), (1, 0), (0, -1), (0, 1) };
                Node endNode = null;
                while (queue.Count > 0)
                {
                    var cur = queue.Dequeue();
                    if (cur.X == ex && cur.Y == ey)
                    {
                        endNode = cur;
                        break;
                    }

                    foreach (var (xo, yo) in nos)
                    {
                        if (!nodes.TryGetValue((cur.X + xo, cur.Y + yo), out Node node))
                            continue;

                        if (node == moving || node.Prev != null || node.Used > cur.Total)
                            continue;

                        node.Prev = cur;
                        queue.Enqueue(node);
                    }
                }

                Trace.Assert(endNode != null);
                List<Node> path = new List<Node>();
                while (endNode.Prev != endNode)
                {
                    path.Add(endNode);
                    endNode = endNode.Prev;
                }

                path.Add(endNode);
                path.Reverse();

                for (int i = 0; i < path.Count - 1; i++)
                {
                    path[i].Used += path[i + 1].Used;
                    path[i + 1].Used = 0;
                }

                return path.Count - 1;
            }
        }

        private class Node
        {
            public Node(int x, int y, int total)
            {
                X = x;
                Y = y;
                Total = total;
            }

            public int X { get; }
            public int Y { get; }
            public int Used { get; set; }
            public int Total { get; }
            public int Avail => Total - Used;
            public Node Prev { get; set; }
        }
    }
}
