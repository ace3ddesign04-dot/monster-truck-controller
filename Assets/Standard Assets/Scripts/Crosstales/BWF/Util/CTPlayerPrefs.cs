using System;
using UnityEngine;

namespace Crosstales.BWF.Util
{
	public static class CTPlayerPrefs
	{
		public static bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		public static void DeleteAll()
		{
			PlayerPrefs.DeleteAll();
		}

		public static void DeleteKey(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		public static void Save()
		{
			PlayerPrefs.Save();
		}

		public static string GetString(string key)
		{
			return PlayerPrefs.GetString(key);
		}

		public static float GetFloat(string key)
		{
			return PlayerPrefs.GetFloat(key);
		}

		public static int GetInt(string key)
		{
			return PlayerPrefs.GetInt(key);
		}

		public static bool GetBool(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			return "true".CTEquals(PlayerPrefs.GetString(key)) ? true : false;
		}

		public static void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		public static void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		public static void SetBool(string key, bool value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			PlayerPrefs.SetString(key, (!value) ? "false" : "true");
		}
	}
}
