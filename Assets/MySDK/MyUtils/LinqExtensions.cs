using System.Collections.Generic;

namespace MyUtils
{
    // 1 Phiên bản ghi đè các Linq để sử dụng mà tạo rác tối thiểu
    public static class LinqExtensions
    {
        public static void EForeach<T>(this IList<T> items, System.Action<T> action)
		{
			for (var i = 0; i < items.Count; i++)
			{
				action(items[i]);
			}
		}

		public static void EWhere<T>(this IList<T> items, System.Func<T, bool> func, ref IList<T> results)
		{
            results.Clear();

			for (var i = 0; i < items.Count; i++)
			{
				T item = items[i];

				if (func(item))
				{
					results.Add(item);
				}
			}
		}

		public static T EAggregate<T>(this IList<T> items, T seed, System.Func<T, T, T> accumulator)
		{
			T value = seed;

			for (var i = 0; i < items.Count; i++)
			{
				value = accumulator(value, items[i]);
			}

			return value;
		}

		public static bool EAll<T>(this IList<T> items, System.Func<T, bool> func)
		{
			for (var i = 0; i < items.Count; i++)
			{
				if (!func(items[i]))
				{
					return false;
				}
			}

			return true;
		}

		public static bool EAny<T> (this IList<T> items, System.Func<T, bool> func)
		{
			for (var i = 0; i < items.Count; i++)
			{
				if (func(items[i]))
				{
					return true;
				}
			}

			return false;
		}

		public static void EDistinct<T>(this IList<T> items, ref IList<T> results)
		{
            results.Clear();

			for (var i = 0; i < items.Count; i++)
			{
				T item = items[i];

				if (!results.Contains(item))
				{
                    results.Add(item);
				}
			}
		}

		public static void EExcept<T>(this IList<T> items, IList<T> second, ref IList<T> results)
		{
			results.Clear();

			HashSet<T> hashSet = second.EToHashSet();

			for (var i = 0; i < items.Count; i++)
			{
				T item = items[i];

				if (!hashSet.Contains(item))
				{
					results.Add(item);
				}
			}
		}

		public static T EFirstOrDefault<T>(this IList<T> items, System.Func<T, bool> func)
		{
			for (var i = 0; i < items.Count; i++)
			{
				if (func(items[i]))
				{
					return items[i];
				}
			}

			return default(T);
		}

		public static int EFindIndex<T>(this IList<T> items, System.Func<T, bool> func)
		{
			for (var i = 0; i < items.Count; i++)
			{
				if (func(items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static T EFind<T>(this IList<T> items, System.Func<T, bool> func)
		{
			for (var i = 0; i < items.Count; i++)
			{
				if (func(items[i]))
				{
					return items[i];
				}
			}

			return default(T);
		}

		public static void ESelect<T, TResult>(this IList<T> items, System.Func<T, TResult> func, ref IList<TResult> results)
        {
            results.Clear();

            for (var i = 0; i < items.Count; i++)
            {
                results.Add(func(items[i]));
            }
        }

		public static HashSet<T> EToHashSet<T>(this IList<T> items)
		{
			HashSet<T> hashSet = new HashSet<T>();

			for (var i = 0; i < items.Count; i++)
			{
				hashSet.Add(items[i]);
			}

			return hashSet;
		}

        public static Dictionary<TKey, T> EToDictionary<T, TKey>(this IList<T> items, System.Func<T, TKey> keySelector)
        {
            Dictionary<TKey, T> dictionary = new Dictionary<TKey, T>(items.Count);

            for (var i = 0; i < items.Count; i++)
            {
                T item = items[i];
                TKey key = keySelector(item);

                if (!dictionary.TryAdd(key, item))
                {
                    Common.LogWarning("Key already exists: " + key);
                }
            }

            return dictionary;
        }

    }
}

