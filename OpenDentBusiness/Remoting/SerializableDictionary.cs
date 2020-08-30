using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
	}

	public static class SerializableDictionaryExtensions
	{
		public static SerializableDictionary<TKey, TValue> ToSerializableDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
		{
			var dictionary = new SerializableDictionary<TKey, TValue>();

			foreach (TSource element in source)
			{
				dictionary.Add(keySelector(element), elementSelector(element));
			}

			return dictionary;
		}
	}
}
