using System.Text;

namespace AdventOfCode
{
	internal static class Day10
	{
		public static int Part1(string input)
		{
			for (int i = 0; i < 40; i++)
				input = Next(input);

			return input.Length;
		}

		private static string Next(string cur)
		{
			StringBuilder sb = new StringBuilder();
			int curPos = 0;
			while (curPos < cur.Length)
			{
				char scan = cur[curPos];
				int num;
				for (num = curPos; num < cur.Length && cur[num] == scan; num++)
				{
				}

				sb.Append(num - curPos);
				sb.Append(scan);
				curPos = num;
			}

			return sb.ToString();
		}
		public static int Part2(string input)
		{
			for (int i = 0; i < 50; i++)
				input = Next(input);

			return input.Length;
		}
	}
}