using Crosstales.BWF.Model;
using Crosstales.BWF.Provider;
using Crosstales.BWF.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Crosstales.BWF.Filter
{
	public class BadWordFilter : BaseFilter
	{
		public string ReplaceCharacters;

		public bool ReplaceLeetSpeak;

		public bool SimpleCheck;

		private readonly List<BadWordProvider> tempBadWordProviderLTR;

		private readonly List<BadWordProvider> tempBadWordProviderRTL;

		private readonly Dictionary<string, Regex> exactBadwordsRegex = new Dictionary<string, Regex>(30);

		private readonly Dictionary<string, List<Regex>> debugExactBadwordsRegex = new Dictionary<string, List<Regex>>(30);

		private readonly Dictionary<string, List<string>> simpleBadwords = new Dictionary<string, List<string>>(30);

		private bool ready;

		private bool readyFirstime;

		private List<BadWordProvider> badWordProviderLTR = new List<BadWordProvider>();

		private List<BadWordProvider> badWordProviderRTL = new List<BadWordProvider>();

		public List<BadWordProvider> BadWordProviderLTR
		{
			get
			{
				return badWordProviderLTR;
			}
			set
			{
				badWordProviderLTR = value;
				if (badWordProviderLTR != null && badWordProviderLTR.Count > 0)
				{
					foreach (BadWordProvider item in badWordProviderLTR)
					{
						if (item != null)
						{
							if (Config.DEBUG_BADWORDS)
							{
								debugExactBadwordsRegex.CTAddRange(item.DebugExactBadwordsRegex);
							}
							else
							{
								exactBadwordsRegex.CTAddRange(item.ExactBadwordsRegex);
							}
							simpleBadwords.CTAddRange(item.SimpleBadwords);
						}
						else if (!Helper.isEditorMode)
						{
							UnityEngine.Debug.LogError("A LTR-BadWordProvider is null!");
						}
					}
					return;
				}
				badWordProviderLTR = new List<BadWordProvider>();
				if (!Helper.isEditorMode)
				{
					UnityEngine.Debug.LogWarning("No 'BadWordProviderLTR' added!" + Environment.NewLine + "If you want to use this functionality, please add your desired 'BadWordProviderLTR' in the editor or script.");
				}
			}
		}

		public List<BadWordProvider> BadWordProviderRTL
		{
			get
			{
				return badWordProviderRTL;
			}
			set
			{
				badWordProviderRTL = value;
				if (badWordProviderRTL != null && badWordProviderRTL.Count > 0)
				{
					foreach (BadWordProvider item in badWordProviderRTL)
					{
						if (item != null)
						{
							if (Config.DEBUG_BADWORDS)
							{
								debugExactBadwordsRegex.CTAddRange(item.DebugExactBadwordsRegex);
							}
							else
							{
								exactBadwordsRegex.CTAddRange(item.ExactBadwordsRegex);
							}
							simpleBadwords.CTAddRange(item.SimpleBadwords);
						}
						else if (!Helper.isEditorMode)
						{
							UnityEngine.Debug.LogError("A RTL-BadWordProvider is null!");
						}
					}
					return;
				}
				badWordProviderRTL = new List<BadWordProvider>();
				if (!Helper.isEditorMode)
				{
					UnityEngine.Debug.LogWarning("No 'BadWordProviderRTL' added!" + Environment.NewLine + "If you want to use this functionality, please add your desired 'BadWordProviderRTL' in the editor or script.");
				}
			}
		}

		public override bool isReady
		{
			get
			{
				bool flag = true;
				if (!ready)
				{
					if (tempBadWordProviderLTR != null)
					{
						foreach (BadWordProvider item in tempBadWordProviderLTR)
						{
							if (item != null && !item.isReady)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag && tempBadWordProviderRTL != null)
					{
						foreach (BadWordProvider item2 in tempBadWordProviderRTL)
						{
							if (item2 != null && !item2.isReady)
							{
								flag = false;
								break;
							}
						}
					}
					if (!readyFirstime && flag)
					{
						BadWordProviderLTR = tempBadWordProviderLTR;
						BadWordProviderRTL = tempBadWordProviderRTL;
						if (BadWordProviderLTR != null)
						{
							foreach (BadWordProvider item3 in BadWordProviderLTR)
							{
								if (item3 != null)
								{
									Source[] sources = item3.Sources;
									foreach (Source source in sources)
									{
										if (!base.sources.ContainsKey(source.Name))
										{
											base.sources.Add(source.Name, source);
										}
									}
								}
							}
						}
						if (BadWordProviderRTL != null)
						{
							foreach (BadWordProvider item4 in BadWordProviderRTL)
							{
								if (item4 != null)
								{
									Source[] sources2 = item4.Sources;
									foreach (Source source2 in sources2)
									{
										if (!base.sources.ContainsKey(source2.Name))
										{
											base.sources.Add(source2.Name, source2);
										}
									}
								}
							}
						}
						readyFirstime = true;
					}
				}
				ready = flag;
				return flag;
			}
		}

		public BadWordFilter(List<BadWordProvider> badWordProviderLTR, List<BadWordProvider> badWordProviderRTL, string replaceCharacters, bool leetSpeak, bool simpleCheck, string markPrefix, string markPostfix)
		{
			tempBadWordProviderLTR = badWordProviderLTR;
			tempBadWordProviderRTL = badWordProviderRTL;
			ReplaceCharacters = replaceCharacters;
			ReplaceLeetSpeak = leetSpeak;
			SimpleCheck = simpleCheck;
			MarkPrefix = markPrefix;
			MarkPostfix = markPostfix;
		}

		public override bool Contains(string testString, params string[] sources)
		{
			bool flag = false;
			if (isReady)
			{
				if (string.IsNullOrEmpty(testString))
				{
					logContains();
				}
				else
				{
					string text = replaceLeetSpeak(testString);
					if (Config.DEBUG_BADWORDS)
					{
						if (sources == null || sources.Length == 0)
						{
							if (SimpleCheck)
							{
								foreach (List<string> value5 in simpleBadwords.Values)
								{
									foreach (string item in value5)
									{
										if (text.CTContains(item))
										{
											UnityEngine.Debug.Log("Test string contains a bad word detected by word '" + item + "'");
											flag = true;
											break;
										}
									}
									if (flag)
									{
										return flag;
									}
								}
								return flag;
							}
							{
								foreach (List<Regex> value6 in debugExactBadwordsRegex.Values)
								{
									foreach (Regex item2 in value6)
									{
										Match match = item2.Match(text);
										if (match.Success)
										{
											UnityEngine.Debug.Log("Test string contains a bad word: '" + match.Value + "' detected by regex '" + item2 + "'");
											flag = true;
											break;
										}
										if (flag)
										{
											break;
										}
									}
								}
								return flag;
							}
						}
						for (int i = 0; i < sources.Length; i++)
						{
							if (flag)
							{
								break;
							}
							List<Regex> value2;
							if (SimpleCheck)
							{
								if (simpleBadwords.TryGetValue(sources[i], out List<string> value))
								{
									foreach (string item3 in value)
									{
										if (text.CTContains(item3))
										{
											UnityEngine.Debug.Log("Test string contains a bad word detected by word '" + item3 + "'' from source '" + sources[i] + "'");
											flag = true;
											break;
										}
									}
									if (flag)
									{
										break;
									}
								}
								else
								{
									logResourceNotFound(sources[i]);
								}
							}
							else if (debugExactBadwordsRegex.TryGetValue(sources[i], out value2))
							{
								foreach (Regex item4 in value2)
								{
									Match match = item4.Match(text);
									if (match.Success)
									{
										UnityEngine.Debug.Log("Test string contains a bad word: '" + match.Value + "' detected by regex '" + item4 + "'' from source '" + sources[i] + "'");
										flag = true;
										break;
									}
								}
							}
							else
							{
								logResourceNotFound(sources[i]);
							}
						}
					}
					else
					{
						if (sources == null || sources.Length == 0)
						{
							if (SimpleCheck)
							{
								foreach (List<string> value7 in simpleBadwords.Values)
								{
									foreach (string item5 in value7)
									{
										if (text.CTContains(item5))
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										return flag;
									}
								}
								return flag;
							}
							{
								foreach (Regex value8 in exactBadwordsRegex.Values)
								{
									if (value8.Match(text).Success)
									{
										return true;
									}
								}
								return flag;
							}
						}
						foreach (string text2 in sources)
						{
							Regex value4;
							if (SimpleCheck)
							{
								if (simpleBadwords.TryGetValue(text2, out List<string> value3))
								{
									foreach (string item6 in value3)
									{
										if (text.CTContains(item6))
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										break;
									}
								}
								else
								{
									logResourceNotFound(text2);
								}
							}
							else if (exactBadwordsRegex.TryGetValue(text2, out value4))
							{
								Match match = value4.Match(text);
								if (match.Success)
								{
									flag = true;
									break;
								}
							}
							else
							{
								logResourceNotFound(text2);
							}
						}
					}
				}
			}
			else
			{
				logFilterNotReady();
			}
			return flag;
		}

		public override List<string> GetAll(string testString, params string[] sources)
		{
			List<string> list = new List<string>();
			if (isReady)
			{
				if (string.IsNullOrEmpty(testString))
				{
					logGetAll();
				}
				else
				{
					string text = replaceLeetSpeak(testString);
					if (Config.DEBUG_BADWORDS)
					{
						if (sources == null || sources.Length == 0)
						{
							if (SimpleCheck)
							{
								foreach (List<string> value5 in simpleBadwords.Values)
								{
									foreach (string item in value5)
									{
										if (text.CTContains(item))
										{
											UnityEngine.Debug.Log("Test string contains a bad word detected by word '" + item + "'");
											if (!list.Contains(item))
											{
												list.Add(item);
											}
										}
									}
								}
							}
							else
							{
								foreach (List<Regex> value6 in debugExactBadwordsRegex.Values)
								{
									foreach (Regex item2 in value6)
									{
										MatchCollection matchCollection = item2.Matches(text);
										IEnumerator enumerator5 = matchCollection.GetEnumerator();
										try
										{
											while (enumerator5.MoveNext())
											{
												Match match = (Match)enumerator5.Current;
												IEnumerator enumerator6 = match.Captures.GetEnumerator();
												try
												{
													while (enumerator6.MoveNext())
													{
														Capture capture = (Capture)enumerator6.Current;
														UnityEngine.Debug.Log("Test string contains a bad word: '" + capture.Value + "' detected by regex '" + item2 + "'");
														if (!list.Contains(capture.Value))
														{
															list.Add(capture.Value);
														}
													}
												}
												finally
												{
													IDisposable disposable;
													if ((disposable = (enumerator6 as IDisposable)) != null)
													{
														disposable.Dispose();
													}
												}
											}
										}
										finally
										{
											IDisposable disposable2;
											if ((disposable2 = (enumerator5 as IDisposable)) != null)
											{
												disposable2.Dispose();
											}
										}
									}
								}
							}
						}
						else
						{
							foreach (string text2 in sources)
							{
								List<Regex> value2;
								if (SimpleCheck)
								{
									if (simpleBadwords.TryGetValue(text2, out List<string> value))
									{
										foreach (string item3 in value)
										{
											if (text.CTContains(item3))
											{
												UnityEngine.Debug.Log("Test string contains a bad word detected by word '" + item3 + "'' from source '" + text2 + "'");
												if (!list.Contains(item3))
												{
													list.Add(item3);
												}
											}
										}
									}
									else
									{
										logResourceNotFound(text2);
									}
								}
								else if (debugExactBadwordsRegex.TryGetValue(text2, out value2))
								{
									foreach (Regex item4 in value2)
									{
										MatchCollection matchCollection2 = item4.Matches(text);
										IEnumerator enumerator9 = matchCollection2.GetEnumerator();
										try
										{
											while (enumerator9.MoveNext())
											{
												Match match2 = (Match)enumerator9.Current;
												IEnumerator enumerator10 = match2.Captures.GetEnumerator();
												try
												{
													while (enumerator10.MoveNext())
													{
														Capture capture2 = (Capture)enumerator10.Current;
														UnityEngine.Debug.Log("Test string contains a bad word: '" + capture2.Value + "' detected by regex '" + item4 + "'' from source '" + text2 + "'");
														if (!list.Contains(capture2.Value))
														{
															list.Add(capture2.Value);
														}
													}
												}
												finally
												{
													IDisposable disposable3;
													if ((disposable3 = (enumerator10 as IDisposable)) != null)
													{
														disposable3.Dispose();
													}
												}
											}
										}
										finally
										{
											IDisposable disposable4;
											if ((disposable4 = (enumerator9 as IDisposable)) != null)
											{
												disposable4.Dispose();
											}
										}
									}
								}
								else
								{
									logResourceNotFound(text2);
								}
							}
						}
					}
					else if (sources == null || sources.Length == 0)
					{
						if (SimpleCheck)
						{
							foreach (List<string> value7 in simpleBadwords.Values)
							{
								foreach (string item5 in value7)
								{
									if (text.CTContains(item5) && !list.Contains(item5))
									{
										list.Add(item5);
									}
								}
							}
						}
						else
						{
							foreach (Regex value8 in exactBadwordsRegex.Values)
							{
								MatchCollection matchCollection3 = value8.Matches(text);
								IEnumerator enumerator14 = matchCollection3.GetEnumerator();
								try
								{
									while (enumerator14.MoveNext())
									{
										Match match3 = (Match)enumerator14.Current;
										IEnumerator enumerator15 = match3.Captures.GetEnumerator();
										try
										{
											while (enumerator15.MoveNext())
											{
												Capture capture3 = (Capture)enumerator15.Current;
												if (!list.Contains(capture3.Value))
												{
													list.Add(capture3.Value);
												}
											}
										}
										finally
										{
											IDisposable disposable5;
											if ((disposable5 = (enumerator15 as IDisposable)) != null)
											{
												disposable5.Dispose();
											}
										}
									}
								}
								finally
								{
									IDisposable disposable6;
									if ((disposable6 = (enumerator14 as IDisposable)) != null)
									{
										disposable6.Dispose();
									}
								}
							}
						}
					}
					else
					{
						foreach (string text3 in sources)
						{
							Regex value4;
							if (SimpleCheck)
							{
								if (simpleBadwords.TryGetValue(text3, out List<string> value3))
								{
									foreach (string item6 in value3)
									{
										if (text.CTContains(item6) && !list.Contains(item6))
										{
											list.Add(item6);
										}
									}
								}
								else
								{
									logResourceNotFound(text3);
								}
							}
							else if (exactBadwordsRegex.TryGetValue(text3, out value4))
							{
								MatchCollection matchCollection4 = value4.Matches(text);
								IEnumerator enumerator17 = matchCollection4.GetEnumerator();
								try
								{
									while (enumerator17.MoveNext())
									{
										Match match4 = (Match)enumerator17.Current;
										IEnumerator enumerator18 = match4.Captures.GetEnumerator();
										try
										{
											while (enumerator18.MoveNext())
											{
												Capture capture4 = (Capture)enumerator18.Current;
												if (!list.Contains(capture4.Value))
												{
													list.Add(capture4.Value);
												}
											}
										}
										finally
										{
											IDisposable disposable7;
											if ((disposable7 = (enumerator18 as IDisposable)) != null)
											{
												disposable7.Dispose();
											}
										}
									}
								}
								finally
								{
									IDisposable disposable8;
									if ((disposable8 = (enumerator17 as IDisposable)) != null)
									{
										disposable8.Dispose();
									}
								}
							}
							else
							{
								logResourceNotFound(text3);
							}
						}
					}
				}
			}
			else
			{
				logFilterNotReady();
			}
			return (from x in list.Distinct()
				orderby x
				select x).ToList();
		}

		public override string ReplaceAll(string testString, params string[] sources)
		{
			string text = replaceLeetSpeak(testString);
			bool flag = false;
			if (isReady)
			{
				if (string.IsNullOrEmpty(testString))
				{
					logReplaceAll();
					text = string.Empty;
				}
				else
				{
					string text2 = replaceLeetSpeak(testString);
					if (SimpleCheck)
					{
						foreach (string item in GetAll(text2, sources))
						{
							text2 = Regex.Replace(text2, item, Helper.CreateReplaceString(ReplaceCharacters, item.Length), RegexOptions.IgnoreCase);
							flag = true;
						}
						text = text2;
					}
					else if (Config.DEBUG_BADWORDS)
					{
						if (sources == null || sources.Length == 0)
						{
							foreach (List<Regex> value3 in debugExactBadwordsRegex.Values)
							{
								foreach (Regex item2 in value3)
								{
									MatchCollection matchCollection = item2.Matches(text2);
									IEnumerator enumerator4 = matchCollection.GetEnumerator();
									try
									{
										while (enumerator4.MoveNext())
										{
											Match match = (Match)enumerator4.Current;
											IEnumerator enumerator5 = match.Captures.GetEnumerator();
											try
											{
												while (enumerator5.MoveNext())
												{
													Capture capture = (Capture)enumerator5.Current;
													UnityEngine.Debug.Log("Test string contains a bad word: '" + capture.Value + "' detected by regex '" + item2 + "'");
													text = text.Replace(capture.Value, Helper.CreateReplaceString(ReplaceCharacters, capture.Value.Length));
													text = text.Replace(capture.Value, Helper.CreateReplaceString(ReplaceCharacters, capture.Value.Length));
													flag = true;
												}
											}
											finally
											{
												IDisposable disposable;
												if ((disposable = (enumerator5 as IDisposable)) != null)
												{
													disposable.Dispose();
												}
											}
										}
									}
									finally
									{
										IDisposable disposable2;
										if ((disposable2 = (enumerator4 as IDisposable)) != null)
										{
											disposable2.Dispose();
										}
									}
								}
							}
						}
						else
						{
							foreach (string text3 in sources)
							{
								if (debugExactBadwordsRegex.TryGetValue(text3, out List<Regex> value))
								{
									foreach (Regex item3 in value)
									{
										MatchCollection matchCollection2 = item3.Matches(text2);
										IEnumerator enumerator7 = matchCollection2.GetEnumerator();
										try
										{
											while (enumerator7.MoveNext())
											{
												Match match2 = (Match)enumerator7.Current;
												IEnumerator enumerator8 = match2.Captures.GetEnumerator();
												try
												{
													while (enumerator8.MoveNext())
													{
														Capture capture2 = (Capture)enumerator8.Current;
														UnityEngine.Debug.Log("Test string contains a bad word: '" + capture2.Value + "' detected by regex '" + item3 + "'' from source '" + text3 + "'");
														text = text.Replace(capture2.Value, Helper.CreateReplaceString(ReplaceCharacters, capture2.Value.Length));
														flag = true;
													}
												}
												finally
												{
													IDisposable disposable3;
													if ((disposable3 = (enumerator8 as IDisposable)) != null)
													{
														disposable3.Dispose();
													}
												}
											}
										}
										finally
										{
											IDisposable disposable4;
											if ((disposable4 = (enumerator7 as IDisposable)) != null)
											{
												disposable4.Dispose();
											}
										}
									}
								}
								else
								{
									logResourceNotFound(text3);
								}
							}
						}
					}
					else if (sources == null || sources.Length == 0)
					{
						foreach (Regex value4 in exactBadwordsRegex.Values)
						{
							MatchCollection matchCollection3 = value4.Matches(text2);
							IEnumerator enumerator10 = matchCollection3.GetEnumerator();
							try
							{
								while (enumerator10.MoveNext())
								{
									Match match3 = (Match)enumerator10.Current;
									IEnumerator enumerator11 = match3.Captures.GetEnumerator();
									try
									{
										while (enumerator11.MoveNext())
										{
											Capture capture3 = (Capture)enumerator11.Current;
											text = text.Replace(capture3.Value, Helper.CreateReplaceString(ReplaceCharacters, capture3.Value.Length));
											flag = true;
										}
									}
									finally
									{
										IDisposable disposable5;
										if ((disposable5 = (enumerator11 as IDisposable)) != null)
										{
											disposable5.Dispose();
										}
									}
								}
							}
							finally
							{
								IDisposable disposable6;
								if ((disposable6 = (enumerator10 as IDisposable)) != null)
								{
									disposable6.Dispose();
								}
							}
						}
					}
					else
					{
						foreach (string text4 in sources)
						{
							if (exactBadwordsRegex.TryGetValue(text4, out Regex value2))
							{
								MatchCollection matchCollection4 = value2.Matches(text2);
								IEnumerator enumerator12 = matchCollection4.GetEnumerator();
								try
								{
									while (enumerator12.MoveNext())
									{
										Match match4 = (Match)enumerator12.Current;
										IEnumerator enumerator13 = match4.Captures.GetEnumerator();
										try
										{
											while (enumerator13.MoveNext())
											{
												Capture capture4 = (Capture)enumerator13.Current;
												text = text.Replace(capture4.Value, Helper.CreateReplaceString(ReplaceCharacters, capture4.Value.Length));
												flag = true;
											}
										}
										finally
										{
											IDisposable disposable7;
											if ((disposable7 = (enumerator13 as IDisposable)) != null)
											{
												disposable7.Dispose();
											}
										}
									}
								}
								finally
								{
									IDisposable disposable8;
									if ((disposable8 = (enumerator12 as IDisposable)) != null)
									{
										disposable8.Dispose();
									}
								}
							}
							else
							{
								logResourceNotFound(text4);
							}
						}
					}
				}
			}
			else
			{
				logFilterNotReady();
			}
			if (flag)
			{
				return text;
			}
			return testString;
		}

		public override string Replace(string text, List<string> badWords)
		{
			string text2 = replaceLeetSpeak(text);
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
							text2 = text2.Replace(badWord, Helper.CreateReplaceString(ReplaceCharacters, badWord.Length));
						}
						return text2;
					}
				}
				UnityEngine.Debug.LogWarning("Parameter 'badWords' is null or empty!" + Environment.NewLine + "=> 'Replace()' will return the original string.");
			}
			return text2;
		}

		protected string replaceLeetSpeak(string input)
		{
			string text = input;
			if (ReplaceLeetSpeak && !string.IsNullOrEmpty(input))
			{
				text = text.Replace("@", "a");
				text = text.Replace("4", "a");
				text = text.Replace("^", "a");
				text = text.Replace("8", "b");
				text = text.Replace("©", "c");
				text = text.Replace('¢', 'c');
				text = text.Replace("€", "e");
				text = text.Replace("3", "e");
				text = text.Replace("£", "e");
				text = text.Replace("ƒ", "f");
				text = text.Replace("6", "g");
				text = text.Replace("9", "g");
				text = text.Replace("#", "h");
				text = text.Replace("1", "i");
				text = text.Replace("!", "i");
				text = text.Replace("|", "i");
				text = text.Replace("0", "o");
				text = text.Replace("2", "r");
				text = text.Replace("®", "r");
				text = text.Replace("$", "s");
				text = text.Replace("5", "s");
				text = text.Replace("§", "s");
				text = text.Replace("7", "t");
				text = text.Replace("+", "t");
				text = text.Replace("†", "t");
				text = text.Replace("¥", "y");
			}
			return text;
		}
	}
}
