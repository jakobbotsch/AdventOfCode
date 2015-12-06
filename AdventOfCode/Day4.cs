using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode
{
	internal static class Day4
	{
		internal static int Part1(string s)
		{
			using (MD5 md5 = MD5.Create())
			{
				for (int i = 1;; i++)
				{
					byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(s + i));
					if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
						return i;
				}
			}
		}

		internal static int Part2(string s)
		{
			using (MD5 md5 = MD5.Create())
			{
				for (int i = 1;; i++)
				{
					byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(s + i));
					if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
						return i;
				}
			}
		}
	}
}