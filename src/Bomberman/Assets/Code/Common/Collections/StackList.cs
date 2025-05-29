using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Collections
{
	[Serializable]
	public class StackList<T> : IReadOnlyCollection<T>
	{
		[SerializeField] List<T> _list;

		public int Count => _list.Count;

		public StackList() =>
			_list = new List<T>();

		public StackList(int capacity) =>
			_list = new List<T>(capacity);

		public StackList<T> Push(T item)
		{
			_list.Add(item);
			return this;
		}

		public T Pop()
		{
			if (_list.Count == 0)
				return default;

			return PopLastValue();
		}

		public bool TryPop(out T value)
		{
			if (_list.Count == 0)
			{
				value = default;
				return false;
			}

			value = PopLastValue();
			return true;
		}

		public bool TryPeek(out T value)
		{
			if (_list.Count == 0)
			{
				value = default;
				return false;
			}

			value = _list[^1];
			return false;
		}

		public bool IsEmpty() =>
			_list.Count == 0;

		public IEnumerator<T> GetEnumerator() =>
			_list.GetEnumerator();

		T PopLastValue()
		{
			var value = _list[^1];
			_list.RemoveAt(_list.Count - 1);
			return value;
		}

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();
	}
}