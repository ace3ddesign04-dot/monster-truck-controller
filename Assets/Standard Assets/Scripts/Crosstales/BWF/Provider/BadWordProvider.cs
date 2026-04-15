using Crosstales.BWF.Model;
using Crosstales.BWF.Util;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Crosstales.BWF.Provider
{
	public abstract class BadWordProvider : BaseProvider
	{
		protected List<BadWords> badwords = new List<BadWords>();

		private const string exactRegexStart = "(?<![\\w\\d])";

		private const string exactRegexEnd = "s?(?![\\w\\d])";

		private Dictionary<string, Regex> exactBadwordsRegex = new Dictionary<string, Regex>();

		private Dictionary<string, List<Regex>> debugExactBadwordsRegex = new Dictionary<string, List<Regex>>();

		private Dictionary<string, List<string>> simpleBadwords = new Dictionary<string, List<string>>();

		public Dictionary<string, Regex> ExactBadwordsRegex
		{
			get
			{
				return exactBadwordsRegex;
			}
			protected set
			{
				exactBadwordsRegex = value;
			}
		}

		public Dictionary<string, List<Regex>> DebugExactBadwordsRegex
		{
			get
			{
				return debugExactBadwordsRegex;
			}
			protected set
			{
				debugExactBadwordsRegex = value;
			}
		}

		public Dictionary<string, List<string>> SimpleBadwords
		{
			get
			{
				return simpleBadwords;
			}
			protected set
			{
				simpleBadwords = value;
			}
		}

		public override void Load()
		{
			if (ClearOnLoad)
			{
				badwords.Clear();
			}
		}

		protected override void init()
		{
			ExactBadwordsRegex.Clear();
			DebugExactBadwordsRegex.Clear();
			SimpleBadwords.Clear();
			if (Config.DEBUG_BADWORDS)
			{
				UnityEngine.Debug.Log("++ BadWordProvider '" + Name + "' started in debug-mode ++");
			}
			foreach (BadWords badword in badwords)
			{
				if (Config.DEBUG_BADWORDS)
				{
					try
					{
						List<Regex> list = new List<Regex>(badword.BadWordList.Count);
						foreach (string badWord in badword.BadWordList)
						{
							list.Add(new Regex("(?<![\\w\\d])" + badWord + "s?(?![\\w\\d])", RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5));
						}
						if (!DebugExactBadwordsRegex.ContainsKey(badword.Source.Name))
						{
							DebugExactBadwordsRegex.Add(badword.Source.Name, list);
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("Could not generate debug regex for source '" + badword.Source.Name + "': " + ex);
						if (Constants.DEV_DEBUG)
						{
							UnityEngine.Debug.Log(badword.BadWordList.CTDump());
						}
					}
				}
				else
				{
					try
					{
						if (!ExactBadwordsRegex.ContainsKey(badword.Source.Name))
						{
							ExactBadwordsRegex.Add(badword.Source.Name, new Regex("(?<![\\w\\d])(" + string.Join("|", badword.BadWordList.ToArray()) + ")s?(?![\\w\\d])", RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5));
						}
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogError("Could not generate exact regex for source '" + badword.Source.Name + "': " + ex2);
						if (Constants.DEV_DEBUG)
						{
							UnityEngine.Debug.Log(badword.BadWordList.CTDump());
						}
					}
				}
				List<string> list2 = new List<string>(badword.BadWordList.Count);
				list2.AddRange(badword.BadWordList);
				if (!SimpleBadwords.ContainsKey(badword.Source.Name))
				{
					SimpleBadwords.Add(badword.Source.Name, list2);
				}
				if (Config.DEBUG_BADWORDS)
				{
					UnityEngine.Debug.Log("Bad word resource '" + badword.Source.Name + "' loaded and " + badword.BadWordList.Count + " entries found.");
				}
			}
			base.isReady = true;
		}
	}
}
