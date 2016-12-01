using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Properties
{
	public class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
	                                               IReadOnlyDictionary<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> _dict;
		private readonly TValue _defaultValue;

		public DefaultDictionary()
		{
			_dict = new Dictionary<TKey, TValue>();
		}

		public DefaultDictionary(TValue defaultValue)
		{
			_dict = new Dictionary<TKey, TValue>();
			_defaultValue = defaultValue;
		}

		public DefaultDictionary(TValue defaultValue, IEqualityComparer<TKey> comparer)
		{
			_dict = new Dictionary<TKey, TValue>(comparer);
			_defaultValue = defaultValue;
		}

		public ICollection<TKey> Keys => _dict.Keys;

		public ICollection<TValue> Values => _dict.Values;

		public int Count => _dict.Count;

		public TValue this[TKey key]
		{
			get
			{
				TValue val;
				TryGetValue(key, out val);
				return val;
			}
			set { _dict[key] = value; }
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (!_dict.TryGetValue(key, out value))
			{
				value = _defaultValue;
				return false;
			}

			return true;
		}

		public void Add(TKey key, TValue value)
		{
			_dict.Add(key, value);
		}

		public bool Remove(TKey key)
		{
			return _dict.Remove(key);
		}

		public bool ContainsKey(TKey key)
		{
			return _dict.ContainsKey(key);
		}

		public void Clear()
		{
			_dict.Clear();
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dict.GetEnumerator();
		}

		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _dict.Values;

		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _dict.Keys;

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_dict).GetEnumerator();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_dict).Add(item);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array,
		                                                    int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_dict).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).Remove(item);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
			=> ((ICollection<KeyValuePair<TKey, TValue>>)_dict).IsReadOnly;
	}
}