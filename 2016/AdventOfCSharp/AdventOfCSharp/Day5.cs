using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCSharp
{
    internal static class Day5
    {
        internal static void Solve1(string input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                string pass = "";
                for (int i = 0; pass.Length < 8; i++)
                {
                    string toHash = input + i;
                    byte[] bytes = Encoding.ASCII.GetBytes(toHash);
                    byte[] hash = md5.ComputeHash(bytes);
                    if (hash[0] == 0 && hash[1] == 0 && (hash[2] >> 4) == 0)
                        pass += (hash[2] & 0xF).ToString("x");
                }

                Console.WriteLine("Pass: {0}", pass);
            }
        }

        internal static void Solve2(string input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                StringBuilder pass = new StringBuilder("________");
                int mask = 0;
                for (int i = 0; mask != 0xFF; i++)
                {
                    string toHash = input + i;
                    byte[] bytes = Encoding.ASCII.GetBytes(toHash);
                    byte[] hash = md5.ComputeHash(bytes);
                    if (hash[0] == 0 && hash[1] == 0 && (hash[2] >> 4) == 0)
                    {
                        int pos = hash[2] & 0xF;
                        int val = hash[3] >> 4;

                        if (pos >= 8 || (mask & (1 << pos)) != 0)
                            continue;

                        mask |= 1 << pos;
                        pass[pos] = val.ToString("x")[0];
                    }
                }

                Console.WriteLine("Pass: {0}", pass);
            }
        }
    }
}
