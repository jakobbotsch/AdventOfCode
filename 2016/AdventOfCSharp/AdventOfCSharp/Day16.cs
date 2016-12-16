using System;
using System.Linq;
using System.Text;

namespace AdventOfCSharp
{
    internal static class Day16
    {
        internal static void Solve(string input)
        {
            const int length = 35651584;
            string a = input;
            while (a.Length < length)
            {
                string b = a;
                a += "0" + new string(b.Reverse().Select(c => c == '0' ? '1' : '0').ToArray());
            }

            a = a.Remove(length);

            while (a.Length % 2 == 0)
            {
                StringBuilder newA = new StringBuilder();
                for (int i = 0; i < a.Length; i += 2)
                {
                    if (a[i] == a[i + 1])
                        newA.Append("1");
                    else
                        newA.Append("0");
                }

                a = newA.ToString();
            }

            Console.WriteLine("Checksum: {0}", a);
        }
    }
}
