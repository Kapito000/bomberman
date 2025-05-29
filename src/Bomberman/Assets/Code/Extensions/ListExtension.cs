using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Extensions
{
	public static class ListExtension
	{
		public static T GetRandom<T>(this IList<T> list)
		{
			var index = Random.Range(0, list.Count);
			return list[index];
		}

		public static T GetRandom<T>(this IList<T> list, out int index)
		{
			index = Random.Range(0, list.Count);
			return list[index];
		}

		public static int GetRandomIndex<T>(this IList<T> list) =>
			Random.Range(0, list.Count);

		public static bool TryGetIndex<T>(this IList<T> list,
			Func<T, bool> predicate, out int index)
		{
			if (predicate == null)
			{
				index = default;
				return false;
			}

			for (var i = 0; i < list.Count; i++)
			{
				if (predicate.Invoke(list[i]))
				{
					index = i;
					return true;
				}
			}

			index = default;
			return false;
		}
	}
}