using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode
{
    internal static class Day20
    {
        public static void Solve1(string input)
        {
            List<Particle> particles = Parse(input);

            Particle lastClosest = null;
            while (true)
            {
                Particle closest = null;
                BigInteger? closestDist = null;
                foreach (Particle p in particles)
                {
                    p.VelocityX += p.AccelerationX;
                    p.VelocityY += p.AccelerationY;
                    p.VelocityZ += p.AccelerationZ;
                    p.PosX += p.VelocityX;
                    p.PosY += p.VelocityY;
                    p.PosZ += p.VelocityZ;

                    BigInteger dist = BigInteger.Abs(p.PosX) + BigInteger.Abs(p.PosY) + BigInteger.Abs(p.PosZ);
                    if (!closestDist.HasValue || dist < closestDist)
                    {
                        closest = p;
                        closestDist = dist;
                    }
                }

                if (lastClosest != closest)
                {
                    Console.WriteLine(closest.Index);
                    lastClosest = closest;
                }
            }
        }

        public static void Solve2(string input)
        {
            List<Particle> particles = Parse(input);
            while (true)
            {
                var seen = new Dictionary<(BigInteger, BigInteger, BigInteger), int>();
                foreach (Particle p in particles)
                {
                    p.VelocityX += p.AccelerationX;
                    p.VelocityY += p.AccelerationY;
                    p.VelocityZ += p.AccelerationZ;
                    p.PosX += p.VelocityX;
                    p.PosY += p.VelocityY;
                    p.PosZ += p.VelocityZ;

                    var t = (p.PosX, p.PosY, p.PosZ);
                    seen.TryGetValue(t, out int nums);
                    nums++;
                    seen[t] = nums;
                }

                particles.RemoveAll(p => seen[(p.PosX, p.PosY, p.PosZ)] > 1);
                Console.WriteLine(particles.Count);
            }
        }

        private static List<Particle> Parse(string input)
        {
            return Util.GetLines(input).Select((s, i) =>
            {
                var nums = Util.GetInts(s);
                return new Particle
                {
                    Index = i,
                    PosX = nums[0],
                    PosY = nums[1],
                    PosZ = nums[2],
                    VelocityX = nums[3],
                    VelocityY = nums[4],
                    VelocityZ = nums[5],
                    AccelerationX = nums[6],
                    AccelerationY = nums[7],
                    AccelerationZ = nums[8],
                };
            }).ToList();
        }

        private class Particle
        {
            public int Index;
            public BigInteger PosX;
            public BigInteger PosY;
            public BigInteger PosZ;
            public BigInteger VelocityX;
            public BigInteger VelocityY;
            public BigInteger VelocityZ;
            public BigInteger AccelerationX;
            public BigInteger AccelerationY;
            public BigInteger AccelerationZ;
        }
    }
}
