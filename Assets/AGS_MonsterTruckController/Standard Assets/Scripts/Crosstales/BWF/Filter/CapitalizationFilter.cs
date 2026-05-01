using Crosstales.BWF.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Crosstales.BWF.Filter
{
	public class CapitalizationFilter : BaseFilter
	{
		private int characterNumber;

		public Regex RegularExpression
		{
			get;
			private set;
		}

		public int CharacterNumber
		{
			get
			{
				return characterNumber;
			}
			set
			{
				if (value < 2)
				{
					characterNumber = 2;
				}
				else
				{
					characterNumber = value;
				}
				RegularExpression = new Regex("\\b\\w*[A-ZÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝ]{" + (characterNumber + 1) + ",}\\w*\\b", RegexOptions.CultureInvariant);
			}
		}

		public override bool isReady => true;

		public CapitalizationFilter(int capitalizationCharsNumber, string markPrefix, string markPostfix)
		{
			CharacterNumber = capitalizationCharsNumber;
			MarkPrefix = markPrefix;
			MarkPostfix = markPostfix;
		}

		public override bool Contains(string testString, params string[] sources)
		{
			bool result = false;
			if (string.IsNullOrEmpty(testString))
			{
				logContains();
			}
			else
			{
				result = RegularExpression.Match(testString).Success;
			}
			return result;
		}

		public override List<string> GetAll(string testString, params string[] sources)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(testString))
			{
				logGetAll();
			}
			else
			{
				MatchCollection matchCollection = RegularExpression.Matches(testString);
				IEnumerator enumerator = matchCollection.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Match match = (Match)enumerator.Current;
						IEnumerator enumerator2 = match.Captures.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								Capture capture = (Capture)enumerator2.Current;
								if (Constants.DEV_DEBUG)
								{
									UnityEngine.Debug.Log("Test string contains an excessive capital word: '" + capture.Value + "'");
								}
								if (!list.Contains(capture.Value))
								{
									list.Add(capture.Value);
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator2 as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
			return (from x in list.Distinct()
				orderby x
				select x).ToList();
		}

		public override string ReplaceAll(string testString, params string[] sources)
		{
			string text = testString;
			if (string.IsNullOrEmpty(testString))
			{
				logReplaceAll();
				return string.Empty;
			}
			MatchCollection matchCollection = RegularExpression.Matches(testString);
			IEnumerator enumerator = matchCollection.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Match match = (Match)enumerator.Current;
					IEnumerator enumerator2 = match.Captures.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Capture capture = (Capture)enumerator2.Current;
							if (Constants.DEV_DEBUG)
							{
								UnityEngine.Debug.Log("Test string contains an excessive capital word: '" + capture.Value + "'");
							}
							text = text.Replace(capture.Value, capture.Value.ToLowerInvariant());
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				return text;
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}

		public override string Replace(string text, List<string> badWords)
		{
			string text2 = text;
			if (string.IsNullOrEmpty(text))
			{
				logReplace();
				text2 = string.Empty;
			}
			else
			{
				if (badWords != null && badWords.Count != 0)
				{
					{
						foreach (string badWord in badWords)
						{
							text2 = text2.Replace(badWord, badWord.ToLowerInvariant());
						}
						return text2;
					}
				}
				UnityEngine.Debug.LogWarning("Parameter 'badWords' is null or empty!" + Environment.NewLine + "=> 'Replace()' will return the original string.");
			}
			return text2;
		}
	}
}
