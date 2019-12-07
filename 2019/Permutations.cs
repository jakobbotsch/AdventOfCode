using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public static class Permutations
    {
	    public static IEnumerable<T[]> Unordered<T>(IEnumerable<T> input)
	    {
		    T[] x = input.ToArray();
		    if (x.Length == 0)
			    yield break;
		    if (x.Length == 1)
		    {
				yield return x;
			    yield break;
		    }

		    int[] c = new int[x.Length];

		    for (int d = 1;; c[d]++)
		    {
			    yield return x;

			    while (d > 1)
				    c[--d] = 0;

			    while (c[d] >= d)
			    {
				    if (++d >= x.Length)
					    yield break;
			    }

			    int i = (d & 1) != 0 ? c[d] : 0;
			    T temp = x[i];
			    x[i] = x[d];
			    x[d] = temp;
		    }
	    }

		public static IEnumerable<T[]> Ordered<T>(IEnumerable<T> input)
		{
			T[] x = input.ToArray();
			yield return x;

			int[] indices = Enumerable.Range(0, x.Length).ToArray();

			while (true)
			{
				int largestSorted = -1;
				for (int i = 0; i < indices.Length - 1; i++)
				{
					if (indices[i] < indices[i + 1])
						largestSorted = i;
				}

				if (largestSorted == -1)
					yield break;

				int largestGreater = largestSorted + 1;
				for (int i = largestSorted + 2; i < indices.Length; i++)
				{
					if (indices[largestSorted] < indices[i])
						largestGreater = i;
				}

				Swap(ref x[largestSorted], ref x[largestGreater]);
				Swap(ref indices[largestSorted], ref indices[largestGreater]);

				int start = largestSorted + 1;
				int end = start + (indices.Length - largestSorted - 1)/2;
				for (int i = start, j = indices.Length - 1; i < end; i++, j--)
				{
					Swap(ref x[i], ref x[j]);
					Swap(ref indices[i], ref indices[j]);
				}

				yield return x;
			}
		}

	    private static void Swap<T>(ref T val1, ref T val2)
	    {
		    T temp = val1;
		    val1 = val2;
		    val2 = temp;
	    }
	}
}
