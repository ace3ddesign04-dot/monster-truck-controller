using Crosstales.BWF.Filter;
using Crosstales.BWF.Model;
using Crosstales.BWF.Provider;
using Crosstales.BWF.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crosstales.BWF.Manager
{
	[DisallowMultipleComponent]
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_manager_1_1_bad_word_manager.html")]
	public class BadWordManager : BaseManager
	{
		[Header("Specific Settings")]
		[Tooltip("Replace characters for bad words (default: *).")]
		public string ReplaceChars = "*";

		[Tooltip("Replace Leet speak in the input string (default: true).")]
		public bool ReplaceLeetSpeak;

		[Tooltip("Use simple detection algorithm. This is the way to check for Chinese, Japanese, Korean and Thai bad words (default: false).")]
		public bool SimpleCheck;

		[Header("Bad Word Providers")]
		[Tooltip("List of all left-to-right providers.")]
		public List<BadWordProvider> BadWordProviderLTR;

		[Tooltip("List of all right-to-left providers.")]
		public List<BadWordProvider> BadWordProviderRTL;

		private static bool initalized;

		private static BadWordFilter filter;

		private static BadWordManager manager;

		private static bool loggedFilterIsNull;

		private static bool loggedOnlyOneInstance;

		private const string clazz = "BadWordManager";

		public static string ReplaceCharacters
		{
			get
			{
				if (filter != null)
				{
					return filter.ReplaceCharacters;
				}
				if (manager != null)
				{
					return manager.ReplaceChars;
				}
				return "*";
			}
			set
			{
				if (filter != null)
				{
					filter.ReplaceCharacters = value;
					manager.ReplaceChars = value;
				}
				else if (manager != null)
				{
					manager.ReplaceChars = value;
				}
			}
		}

		public static bool isReplaceLeetSpeak
		{
			get
			{
				if (filter != null)
				{
					return filter.ReplaceLeetSpeak;
				}
				if (manager != null)
				{
					return manager.ReplaceLeetSpeak;
				}
				return false;
			}
			set
			{
				if (filter != null)
				{
					filter.ReplaceLeetSpeak = value;
					manager.ReplaceLeetSpeak = value;
				}
				else if (manager != null)
				{
					manager.ReplaceLeetSpeak = value;
				}
			}
		}

		public static bool isSimpleCheck
		{
			get
			{
				if (filter != null)
				{
					return filter.SimpleCheck;
				}
				if (manager != null)
				{
					return manager.SimpleCheck;
				}
				return false;
			}
			set
			{
				if (filter != null)
				{
					filter.SimpleCheck = value;
					manager.SimpleCheck = value;
				}
				else if (manager != null)
				{
					manager.SimpleCheck = value;
				}
			}
		}

		public static bool isReady
		{
			get
			{
				bool result = false;
				if (filter != null)
				{
					result = filter.isReady;
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
				return result;
			}
		}

		public static List<Source> Sources
		{
			get
			{
				List<Source> result = new List<Source>();
				if (filter != null)
				{
					result = filter.Sources;
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
				return result;
			}
		}

		public void OnEnable()
		{
			if (Helper.isEditorMode || !initalized)
			{
				manager = this;
				Load();
				if (!Helper.isEditorMode && DontDestroy)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.transform.root.gameObject);
					initalized = true;
				}
			}
			else if (!Helper.isEditorMode && DontDestroy && manager != this)
			{
				if (!loggedOnlyOneInstance)
				{
					loggedOnlyOneInstance = true;
					UnityEngine.Debug.LogWarning("Only one active instance of 'BadWordManager' allowed in all scenes!" + Environment.NewLine + "This object will now be destroyed.");
				}
				UnityEngine.Object.Destroy(base.transform.root.gameObject, 0.2f);
			}
		}

		public static void Load()
		{
			if (manager != null)
			{
				filter = new BadWordFilter(manager.BadWordProviderLTR, manager.BadWordProviderRTL, manager.ReplaceChars, manager.ReplaceLeetSpeak, manager.SimpleCheck, manager.MarkPrefix, manager.MarkPostfix);
			}
		}

		public static bool Contains(string testString, params string[] sources)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(testString))
			{
				if (filter != null)
				{
					result = filter.Contains(testString, sources);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static void ContainsMT(out bool result, string testString, params string[] sources)
		{
			result = Contains(testString, sources);
		}

		public static List<string> GetAll(string testString, params string[] sources)
		{
			List<string> result = new List<string>();
			if (!string.IsNullOrEmpty(testString))
			{
				if (filter != null)
				{
					result = filter.GetAll(testString, sources);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static void GetAllMT(out List<string> result, string testString, params string[] sources)
		{
			result = GetAll(testString, sources);
		}

		public static string ReplaceAll(string testString, params string[] sources)
		{
			string result = testString;
			if (!string.IsNullOrEmpty(testString))
			{
				if (filter != null)
				{
					result = filter.ReplaceAll(testString, sources);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static void ReplaceAllMT(out string result, string testString, params string[] sources)
		{
			result = ReplaceAll(testString, sources);
		}

		public static string Replace(string text, List<string> badWords)
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Replace(text, badWords);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static string Mark(string text, List<string> badWords, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Mark(text, badWords, prefix, postfix);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		public static string Unmark(string text, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Unmark(text, prefix, postfix);
				}
				else
				{
					logFilterIsNull("BadWordManager");
				}
			}
			return result;
		}

		private static void logFilterIsNull(string clazz)
		{
			if (!loggedFilterIsNull)
			{
				UnityEngine.Debug.LogWarning("'filter' is null!" + Environment.NewLine + "Did you add the '" + clazz + "' to the current scene?");
				loggedFilterIsNull = true;
			}
		}
	}
}
