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
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_manager_1_1_domain_manager.html")]
	public class DomainManager : BaseManager
	{
		[Header("Specific Settings")]
		[Tooltip("Replace characters for domains (default: *).")]
		public string ReplaceChars = "*";

		[Header("Domain Providers")]
		[Tooltip("List of all domain providers.")]
		public List<DomainProvider> DomainProvider;

		private static bool initalized;

		private static DomainFilter filter;

		private static DomainManager manager;

		private static bool loggedFilterIsNull;

		private static bool loggedOnlyOneInstance;

		private const string clazz = "DomainManager";

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
					logFilterIsNull("DomainManager");
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
					logFilterIsNull("DomainManager");
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
					UnityEngine.Debug.LogWarning("Only one active instance of 'DomainManager' allowed in all scenes!" + Environment.NewLine + "This object will now be destroyed.");
				}
				UnityEngine.Object.Destroy(base.transform.root.gameObject, 0.2f);
			}
		}

		public static void Load()
		{
			if (manager != null)
			{
				filter = new DomainFilter(manager.DomainProvider, manager.ReplaceChars, manager.MarkPrefix, manager.MarkPostfix);
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
					logFilterIsNull("DomainManager");
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
					logFilterIsNull("DomainManager");
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
					logFilterIsNull("DomainManager");
				}
			}
			return result;
		}

		public static void ReplaceAllMT(out string result, string testString, params string[] sources)
		{
			result = ReplaceAll(testString, sources);
		}

		public static string Replace(string text, List<string> domains)
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Replace(text, domains);
				}
				else
				{
					logFilterIsNull("DomainManager");
				}
			}
			return result;
		}

		public static string Mark(string text, List<string> domains, string prefix = "<b><color=red>", string postfix = "</color></b>")
		{
			string result = text;
			if (!string.IsNullOrEmpty(text))
			{
				if (filter != null)
				{
					result = filter.Mark(text, domains, prefix, postfix);
				}
				else
				{
					logFilterIsNull("DomainManager");
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
					logFilterIsNull("DomainManager");
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
