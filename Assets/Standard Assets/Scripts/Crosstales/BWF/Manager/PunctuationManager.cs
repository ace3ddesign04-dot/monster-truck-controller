using Crosstales.BWF.Filter;
using Crosstales.BWF.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crosstales.BWF.Manager
{
	[DisallowMultipleComponent]
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_manager_1_1_punctuation_manager.html")]
	public class PunctuationManager : BaseManager
	{
		[Header("Specific Settings")]
		[Tooltip("Defines the number of allowed punctuation letters in a row (default: 3).")]
		public int PunctuationCharsNumber = 3;

		private static bool initalized;

		private static PunctuationFilter filter;

		private static PunctuationManager manager;

		private static bool loggedFilterIsNull;

		private static bool loggedOnlyOneInstance;

		private const string clazz = "PunctuationManager";

		public static int CharacterNumber
		{
			get
			{
				if (filter != null)
				{
					return filter.CharacterNumber;
				}
				if (manager != null)
				{
					return manager.PunctuationCharsNumber;
				}
				return 3;
			}
			set
			{
				int num = value;
				if (num < 2)
				{
					num = 2;
				}
				if (filter != null)
				{
					filter.CharacterNumber = num;
					manager.PunctuationCharsNumber = num;
				}
				else if (manager != null)
				{
					manager.PunctuationCharsNumber = num;
				}
			}
		}

		public static bool isReady => filter.isReady;

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
					UnityEngine.Debug.LogWarning("Only one active instance of 'PunctuationManager' allowed in all scenes!" + Environment.NewLine + "This object will now be destroyed.");
				}
				UnityEngine.Object.Destroy(base.transform.root.gameObject, 0.2f);
			}
		}

		public void OnValidate()
		{
			if (PunctuationCharsNumber < 2)
			{
				PunctuationCharsNumber = 2;
			}
		}

		public static void Load()
		{
			if (manager != null)
			{
				filter = new PunctuationFilter(manager.PunctuationCharsNumber, manager.MarkPrefix, manager.MarkPostfix);
			}
		}

		public static bool Contains(string testString)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(testString))
			{
				if (filter != null)
				{
					result = filter.Contains(testString);
				}
				else
				{
					logFilterIsNull("PunctuationManager");
				}
			}
			return result;
		}

		public static void ContainsMT(out bool result, string testString)
		{
			result = Contains(testString);
		}

		public static List<string> GetAll(string testString)
		{
			List<string> result = new List<string>();
			if (!string.IsNullOrEmpty(testString))
			{
				if (filter != null)
				{
					result = filter.GetAll(testString);
				}
				else
				{
					logFilterIsNull("PunctuationManager");
				}
			}
			return result;
		}

		public static void GetAllMT(out List<string> result, string testString)
		{
			result = GetAll(testString);
		}

		public static string ReplaceAll(string testString)
		{
			string result = testString;
			if (!string.IsNullOrEmpty(testString))
			{
				if (filter != null)
				{
					result = filter.ReplaceAll(testString);
				}
				else
				{
					logFilterIsNull("PunctuationManager");
				}
			}
			return result;
		}

		public static void ReplaceAllMT(out string result, string testString)
		{
			result = ReplaceAll(testString);
		}

		public static string Replace(string text, List<string> punctuations)
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Replace(text, punctuations);
				}
				else
				{
					logFilterIsNull("PunctuationManager");
				}
			}
			return result;
		}

		public static string Mark(string text, List<string> punctuations, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Mark(text, punctuations, prefix, postfix);
				}
				else
				{
					logFilterIsNull("PunctuationManager");
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
					logFilterIsNull("PunctuationManager");
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
