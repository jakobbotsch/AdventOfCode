using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCSharp
{
    internal static class Day14
    {
        internal static void Solve(string input)
        {
            const int stretch = 2017;

            Dictionary<int, string> hashes = new Dictionary<int, string>();
            int found = 0;
            using (MD5 md5 = MD5.Create())
            {
                string GetHash(int index)
                {
                    if (hashes.TryGetValue(index, out string val))
                        return val;

                    StringBuilder sb = new StringBuilder();
                    var str = input + index;
                    for (int i = 0; i < stretch; i++)
                    {
                        var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
                        sb.Clear();
                        for (int j = 0; j < hash.Length; j++)
                        {
                            sb.Append(hash[j].ToString("x2"));
                        }

                        str = sb.ToString();
                    }
                    hashes.Add(index, str);
                    return str;
                }

                for (int i = 0; ; i++)
                {
                    var str = GetHash(i);

                    for (int j = 0; j < str.Length - 2; j++)
                    {
                        if (str[j] != str[j + 1] || str[j] != str[j + 2])
                            continue;

                        for (int k = 0; k < 1000; k++)
                        {
                            var str2 = GetHash(i + k + 1);
                            for (int l = 0; l < str2.Length - 4; l++)
                            {
                                if (str2[l] == str[j] && str2[l + 1] == str[j] && str2[l + 2] == str[j] &&
                                    str2[l + 3] == str[j] && str2[l + 4] == str[j])
                                {
                                    Console.WriteLine("{0} matches at {1}", i, i + k + 1);
                                    if (++found == 64)
                                    {
                                        Console.WriteLine("Index: {0}", i);
                                        return;
                                    }

                                    goto NextHash;
                                }
                            }
                        }

                        break;
                    }

                    NextHash:
                    ;
                }
            }
        }
    }
}
