using Crosstales.BWF.Filter;
using Crosstales.BWF.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crosstales.BWF.Manager
{
	[DisallowMultipleComponent]
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_manager_1_1_capitalization_manager.html")]
	public class CapitalizationManager : BaseManager
	{
		[Header("Specific Settings")]
		[Tooltip("Defines the number of allowed capital letters in a row. (default: 3).")]
		public int CapitalizationCharsNumber = 3;

		private static bool initalized;

		private static CapitalizationFilter filter;

		private static CapitalizationManager manager;

		private static bool loggedFilterIsNull;

		private static bool loggedOnlyOneInstance;

		private const string clazz = "CapitalizationManager";

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
					return manager.CapitalizationCharsNumber;
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
					manager.CapitalizationCharsNumber = num;
				}
				else if (manager != null)
				{
					manager.CapitalizationCharsNumber = num;
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
					UnityEngine.Debug.LogWarning("Only one active instance of 'CapitalizationManager' allowed in all scenes!" + Environment.NewLine + "This object will now be destroyed.");
				}
				UnityEngine.Object.Destroy(base.transform.root.gameObject, 0.2f);
			}
		}

		public void OnValidate()
		{
			if (CapitalizationCharsNumber < 2)
			{
				CapitalizationCharsNumber = 2;
			}
		}

		public static void Load()
		{
			if (manager != null)
			{
				filter = new CapitalizationFilter(manager.CapitalizationCharsNumber, manager.MarkPrefix, manager.MarkPostfix);
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
					logFilterIsNull("CapitalizationManager");
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
					logFilterIsNull("CapitalizationManager");
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
					logFilterIsNull("CapitalizationManager");
				}
			}
			return result;
		}

		public static void ReplaceAllMT(out string result, string testString)
		{
			result = ReplaceAll(testString);
		}

		public static string Replace(string text, List<string> capitalWords)
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Replace(text, capitalWords);
				}
				else
				{
					logFilterIsNull("CapitalizationManager");
				}
			}
			return result;
		}

		public static string Mark(string text, List<string> capitalWords, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Mark(text, capitalWords, prefix, postfix);
				}
				else
				{
					logFilterIsNull("CapitalizationManager");
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
					logFilterIsNull("CapitalizationManager");
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
