using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

public class Program
{
    private class Cart
    {
        public int X, Y;
        public int DX, DY;
        public int Turns;

        public Cart(int x, int y, int dx, int dy)
        {
            X = x;
            Y = y;
            DX = dx;
            DY = dy;
        }
    }

    public static void Main()
    {
        char[] getTrack(char[] row)
            => new string(row)
               .Replace('>', '-')
               .Replace('<', '-')
               .Replace('^', '|')
               .Replace('v', '|')
               .ToCharArray();

        char[][] view = File.ReadAllLines("day13.txt").Select(s => s.ToCharArray()).ToArray();
        char[][] tracks = view.Select(getTrack).ToArray();
        List<Cart> carts = new List<Cart>();
        for (int y = 0; y < view.Length; y++)
        {
            for (int x = 0; x < view[y].Length; x++)
            {
                (int dx, int dy) = (-1, -1);
                switch (view[y][x])
                {
                    case '<': (dx, dy) = (-1, 0); break;
                    case '>': (dx, dy) = (1, 0); break;
                    case '^': (dx, dy) = (0, -1); break;
                    case 'v': (dx, dy) = (0, 1); break;
                    default: continue;
                }

                carts.Add(new Cart(x, y, dx, dy));
            }
        }

        bool part1 = true;
        while (true)
        {
            carts = carts.OrderBy(c => c.X).ThenBy(c => c.Y).ToList();

            for (int i = 0; i < carts.Count; i++)
            {
                Cart c = carts[i];
                (int nx, int ny) = (c.X + c.DX, c.Y + c.DY);
                int collision = carts.FindIndex(oc => oc.X == nx && oc.Y == ny);
                if (collision != -1)
                {
                    if (part1)
                    {
                        Console.WriteLine((nx, ny));
                        part1 = false;
                    }

                    carts.RemoveAt(collision > i ? collision : i);
                    carts.RemoveAt(collision > i ? i : collision);
                    i -= collision < i ? 2 : 1;
                    continue;
                }

                (c.X, c.Y) = (nx, ny);
                switch (tracks[ny][nx])
                {
                    case '/': (c.DX, c.DY) = (-c.DY, -c.DX); break;
                    case '\\': (c.DX, c.DY) = (c.DY, c.DX); break;
                    case '+':
                        if (c.Turns % 3 == 0)
                            (c.DX, c.DY) = (c.DY, -c.DX);
                        else if (c.Turns % 3 == 2)
                            (c.DX, c.DY) = (-c.DY, c.DX);
                        c.Turns++;
                        break;
                }
            }

            if (carts.Count == 1)
            {
                Console.WriteLine((carts[0].X, carts[0].Y));
                break;
            }
        }
    }
}
