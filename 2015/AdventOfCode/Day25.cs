namespace AdventOfCode
{
	internal static class Day25
	{
		public static long Part1()
		{
			int row = 1, column = 1;
			long result = 20151125;

			while (row != 3010 || column != 3019)
			{
				result *= 252533;
				result %= 33554393;

				column++;
				row--;

				if (row <= 0)
				{
					row = column;
					column = 1;
				}
			}

			return result;
		}
	}
}