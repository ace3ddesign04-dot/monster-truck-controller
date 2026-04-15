using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Crosstales.BWF
{
	public static class CTExtensionMethods
	{
		private static readonly System.Random rd = new System.Random();

		public static void CTAddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (KeyValuePair<T, S> item in collection)
			{
				if (!source.ContainsKey(item.Key))
				{
					source.Add(item.Key, item.Value);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Duplicate key found: " + item.Key);
				}
			}
		}

		public static bool CTEquals(this string str, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (toCheck == null)
			{
				throw new ArgumentNullException("toCheck");
			}
			return str.Equals(toCheck, comp);
		}

		public static bool CTContains(this string str, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (toCheck == null)
			{
				throw new ArgumentNullException("toCheck");
			}
			return str.IndexOf(toCheck, comp) >= 0;
		}

		public static bool CTContainsAny(this string str, string searchTerms, char splitChar = ' ')
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (string.IsNullOrEmpty(searchTerms))
			{
				return true;
			}
			char[] separator = new char[1]
			{
				splitChar
			};
			return searchTerms.Split(separator, StringSplitOptions.RemoveEmptyEntries).Any((string searchTerm) => str.CTContains(searchTerm));
		}

		public static bool CTContainsAll(this string str, string searchTerms, char splitChar = ' ')
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (string.IsNullOrEmpty(searchTerms))
			{
				return true;
			}
			char[] separator = new char[1]
			{
				splitChar
			};
			return searchTerms.Split(separator, StringSplitOptions.RemoveEmptyEntries).All((string searchTerm) => str.CTContains(searchTerm));
		}

		public static void CTShuffle<T>(this IList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			int num = list.Count;
			while (num > 1)
			{
				int index = rd.Next(num--);
				T value = list[num];
				list[num] = list[index];
				list[index] = value;
			}
		}

		public static void CTShuffle<T>(this T[] array)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			int num = array.Length;
			while (num > 1)
			{
				int num3 = rd.Next(num--);
				T val = array[num];
				array[num] = array[num3];
				array[num3] = val;
			}
		}

		public static string CTDump<T>(this T[] array)
		{
			if (array == null || array.Length <= 0)
			{
				throw new ArgumentNullException("array");
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				T val = array[i];
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(val.ToString());
			}
			return stringBuilder.ToString();
		}

		public static string CTDump<T>(this List<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (T item in list)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(item.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
