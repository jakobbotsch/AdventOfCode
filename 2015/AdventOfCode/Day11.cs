using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace AdventOfCode
{
	internal static class Day11
	{
		public static string Part1(string input)
		{
			do
			{
				input = Next(input);
			} while (!IsValid(input));

			return input;
		}

		private static bool IsValid(string pass)
		{
			Func<bool> hasRun = () =>
			                    {
				                    for (int i = 0; i < pass.Length - 2; i++)
				                    {
					                    if (pass[i] == pass[i + 1] - 1 && pass[i + 1] == pass[i + 2] - 1)
						                    return true;
				                    }

				                    return false;
			                    };

			if (!hasRun())
				return false;

			if (pass.Any(c => c == 'i' || c == 'o' || c == 'l'))
				return false;

			for (int i = 0; i < pass.Length - 1; i++)
			{
				if (pass[i] != pass[i + 1])
					continue;

				for (int j = i + 2; j < pass.Length - 1; j++)
				{
					if (pass[j] == pass[j + 1])
						return true;
				}
			}

			return false;
		}

		private static string Next(string input)
		{
			StringBuilder sb = new StringBuilder(input);

			for (int i = input.Length - 1; i >= 0; i--)
			{
				if (sb[i] == 'z')
				{
					sb[i] = 'a';
					continue;
				}

				sb[i]++;
				break;
			}

			return sb.ToString();
		}

		public static string Part2(string input)
		{
			do
			{
				input = Next(input);
			} while (!IsValid(input));

			do
			{
				input = Next(input);
			} while (!IsValid(input));

			return input;
		}
	}
}