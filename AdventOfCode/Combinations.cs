using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode
{
	public struct Combinations<T> : IEnumerable<T[]>
	{
		private readonly IList<T> _values;
		private readonly int _count;

		public Combinations(IList<T> values, int count)
		{
			_values = values;
			_count = count;
		}

		public IEnumerator<T[]> GetEnumerator()
		{
			T[] result = new T[_count];
			Stack<int> stack = new Stack<int>();
			stack.Push(0);

			while (stack.Count > 0)
			{
				int index = stack.Count - 1;
				int value = stack.Pop();

				while (value < _values.Count)
				{
					result[index++] = _values[value++];
					stack.Push(value);
					if (index == _count)
					{
						yield return result;
						break;
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}