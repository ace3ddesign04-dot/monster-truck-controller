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
	public class DomainFilter : BaseFilter
	{
		public string ReplaceCharacters;

		private List<DomainProvider> domainProvider = new List<DomainProvider>();

		private readonly List<DomainProvider> tempDomainProvider;

		private readonly Dictionary<string, Regex> domainsRegex = new Dictionary<string, Regex>();

		private readonly Dictionary<string, List<Regex>> debugDomainsRegex = new Dictionary<string, List<Regex>>();

		private bool ready;

		private bool readyFirstime;

		public List<DomainProvider> DomainProvider
		{
			get
			{
				return domainProvider;
			}
			set
			{
				domainProvider = value;
				if (domainProvider != null && domainProvider.Count > 0)
				{
					foreach (DomainProvider item in domainProvider)
					{
						if (item != null)
						{
							if (Config.DEBUG_DOMAINS)
							{
								debugDomainsRegex.CTAddRange(item.DebugDomainsRegex);
							}
							else
							{
								domainsRegex.CTAddRange(item.DomainsRegex);
							}
						}
						else if (!Helper.isEditorMode)
						{
							UnityEngine.Debug.LogError("DomainProvider is null!");
						}
					}
					return;
				}
				domainProvider = new List<DomainProvider>();
				if (!Helper.isEditorMode)
				{
					UnityEngine.Debug.LogWarning("No 'DomainProvider' added!" + Environment.NewLine + "If you want to use this functionality, please add your desired 'DomainProvider' in the editor or script.");
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
					if (tempDomainProvider != null)
					{
						foreach (DomainProvider item in tempDomainProvider)
						{
							if (item != null && !item.isReady)
							{
								flag = false;
								break;
							}
						}
					}
					if (!readyFirstime && flag)
					{
						DomainProvider = tempDomainProvider;
						if (DomainProvider != null)
						{
							foreach (DomainProvider item2 in DomainProvider)
							{
								if (item2 != null)
								{
									Source[] sources = item2.Sources;
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
						readyFirstime = true;
					}
				}
				ready = flag;
				return flag;
			}
		}

		public DomainFilter(List<DomainProvider> domainProvider, string replaceCharacters, string markPrefix, string markPostfix)
		{
			tempDomainProvider = domainProvider;
			ReplaceCharacters = replaceCharacters;
			MarkPrefix = markPrefix;
			MarkPostfix = markPostfix;
		}

		public override bool Contains(string testString, params string[] sources)
		{
			bool result = false;
			if (isReady)
			{
				if (string.IsNullOrEmpty(testString))
				{
					logContains();
				}
				else if (Config.DEBUG_DOMAINS)
				{
					if (sources == null || sources.Length == 0)
					{
						foreach (List<Regex> value3 in debugDomainsRegex.Values)
						{
							foreach (Regex item in value3)
							{
								Match match = item.Match(testString);
								if (match.Success)
								{
									UnityEngine.Debug.Log("Test string contains a domain: '" + match.Value + "' detected by regex '" + item + "'");
									result = true;
									break;
								}
							}
						}
						return result;
					}
					foreach (string text in sources)
					{
						if (debugDomainsRegex.TryGetValue(text, out List<Regex> value))
						{
							foreach (Regex item2 in value)
							{
								Match match2 = item2.Match(testString);
								if (match2.Success)
								{
									UnityEngine.Debug.Log("Test string contains a domain: '" + match2.Value + "' detected by regex '" + item2 + "'' from source '" + text + "'");
									result = true;
									break;
								}
							}
						}
						else
						{
							logResourceNotFound(text);
						}
					}
				}
				else
				{
					if (sources == null || sources.Length == 0)
					{
						foreach (Regex value4 in domainsRegex.Values)
						{
							if (value4.Match(testString).Success)
							{
								return true;
							}
						}
						return result;
					}
					foreach (string text2 in sources)
					{
						if (domainsRegex.TryGetValue(text2, out Regex value2))
						{
							Match match3 = value2.Match(testString);
							if (match3.Success)
							{
								result = true;
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
			else
			{
				logFilterNotReady();
			}
			return result;
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
				else if (Config.DEBUG_DOMAINS)
				{
					if (sources == null || sources.Length == 0)
					{
						foreach (List<Regex> value3 in debugDomainsRegex.Values)
						{
							foreach (Regex item in value3)
							{
								MatchCollection matchCollection = item.Matches(testString);
								IEnumerator enumerator3 = matchCollection.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										Match match = (Match)enumerator3.Current;
										IEnumerator enumerator4 = match.Captures.GetEnumerator();
										try
										{
											while (enumerator4.MoveNext())
											{
												Capture capture = (Capture)enumerator4.Current;
												UnityEngine.Debug.Log("Test string contains a domain: '" + capture.Value + "' detected by regex '" + item + "'");
												if (!list.Contains(capture.Value))
												{
													list.Add(capture.Value);
												}
											}
										}
										finally
										{
											IDisposable disposable;
											if ((disposable = (enumerator4 as IDisposable)) != null)
											{
												disposable.Dispose();
											}
										}
									}
								}
								finally
								{
									IDisposable disposable2;
									if ((disposable2 = (enumerator3 as IDisposable)) != null)
									{
										disposable2.Dispose();
									}
								}
							}
						}
					}
					else
					{
						foreach (string text in sources)
						{
							if (debugDomainsRegex.TryGetValue(text, out List<Regex> value))
							{
								foreach (Regex item2 in value)
								{
									MatchCollection matchCollection2 = item2.Matches(testString);
									IEnumerator enumerator6 = matchCollection2.GetEnumerator();
									try
									{
										while (enumerator6.MoveNext())
										{
											Match match2 = (Match)enumerator6.Current;
											IEnumerator enumerator7 = match2.Captures.GetEnumerator();
											try
											{
												while (enumerator7.MoveNext())
												{
													Capture capture2 = (Capture)enumerator7.Current;
													UnityEngine.Debug.Log("Test string contains a domain: '" + capture2.Value + "' detected by regex '" + item2 + "'' from source '" + text + "'");
													if (!list.Contains(capture2.Value))
													{
														list.Add(capture2.Value);
													}
												}
											}
											finally
											{
												IDisposable disposable3;
												if ((disposable3 = (enumerator7 as IDisposable)) != null)
												{
													disposable3.Dispose();
												}
											}
										}
									}
									finally
									{
										IDisposable disposable4;
										if ((disposable4 = (enumerator6 as IDisposable)) != null)
										{
											disposable4.Dispose();
										}
									}
								}
							}
							else
							{
								logResourceNotFound(text);
							}
						}
					}
				}
				else if (sources == null || sources.Length == 0)
				{
					foreach (Regex value4 in domainsRegex.Values)
					{
						MatchCollection matchCollection3 = value4.Matches(testString);
						IEnumerator enumerator9 = matchCollection3.GetEnumerator();
						try
						{
							while (enumerator9.MoveNext())
							{
								Match match3 = (Match)enumerator9.Current;
								IEnumerator enumerator10 = match3.Captures.GetEnumerator();
								try
								{
									while (enumerator10.MoveNext())
									{
										Capture capture3 = (Capture)enumerator10.Current;
										if (!list.Contains(capture3.Value))
										{
											list.Add(capture3.Value);
										}
									}
								}
								finally
								{
									IDisposable disposable5;
									if ((disposable5 = (enumerator10 as IDisposable)) != null)
									{
										disposable5.Dispose();
									}
								}
							}
						}
						finally
						{
							IDisposable disposable6;
							if ((disposable6 = (enumerator9 as IDisposable)) != null)
							{
								disposable6.Dispose();
							}
						}
					}
				}
				else
				{
					foreach (string text2 in sources)
					{
						if (domainsRegex.TryGetValue(text2, out Regex value2))
						{
							MatchCollection matchCollection4 = value2.Matches(testString);
							IEnumerator enumerator11 = matchCollection4.GetEnumerator();
							try
							{
								while (enumerator11.MoveNext())
								{
									Match match4 = (Match)enumerator11.Current;
									IEnumerator enumerator12 = match4.Captures.GetEnumerator();
									try
									{
										while (enumerator12.MoveNext())
										{
											Capture capture4 = (Capture)enumerator12.Current;
											if (!list.Contains(capture4.Value))
											{
												list.Add(capture4.Value);
											}
										}
									}
									finally
									{
										IDisposable disposable7;
										if ((disposable7 = (enumerator12 as IDisposable)) != null)
										{
											disposable7.Dispose();
										}
									}
								}
							}
							finally
							{
								IDisposable disposable8;
								if ((disposable8 = (enumerator11 as IDisposable)) != null)
								{
									disposable8.Dispose();
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
			string text = testString;
			if (isReady)
			{
				if (string.IsNullOrEmpty(testString))
				{
					logReplaceAll();
					text = string.Empty;
				}
				else if (Config.DEBUG_DOMAINS)
				{
					if (sources == null || sources.Length == 0)
					{
						foreach (List<Regex> value3 in debugDomainsRegex.Values)
						{
							foreach (Regex item in value3)
							{
								MatchCollection matchCollection = item.Matches(testString);
								IEnumerator enumerator3 = matchCollection.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										Match match = (Match)enumerator3.Current;
										IEnumerator enumerator4 = match.Captures.GetEnumerator();
										try
										{
											while (enumerator4.MoveNext())
											{
												Capture capture = (Capture)enumerator4.Current;
												UnityEngine.Debug.Log("Test string contains a domain: '" + capture.Value + "' detected by regex '" + item + "'");
												text = text.Replace(capture.Value, Helper.CreateReplaceString(ReplaceCharacters, capture.Value.Length));
											}
										}
										finally
										{
											IDisposable disposable;
											if ((disposable = (enumerator4 as IDisposable)) != null)
											{
												disposable.Dispose();
											}
										}
									}
								}
								finally
								{
									IDisposable disposable2;
									if ((disposable2 = (enumerator3 as IDisposable)) != null)
									{
										disposable2.Dispose();
									}
								}
							}
						}
						return text;
					}
					foreach (string text2 in sources)
					{
						if (debugDomainsRegex.TryGetValue(text2, out List<Regex> value))
						{
							foreach (Regex item2 in value)
							{
								MatchCollection matchCollection2 = item2.Matches(testString);
								IEnumerator enumerator6 = matchCollection2.GetEnumerator();
								try
								{
									while (enumerator6.MoveNext())
									{
										Match match2 = (Match)enumerator6.Current;
										IEnumerator enumerator7 = match2.Captures.GetEnumerator();
										try
										{
											while (enumerator7.MoveNext())
											{
												Capture capture2 = (Capture)enumerator7.Current;
												UnityEngine.Debug.Log("Test string contains a domain: '" + capture2.Value + "' detected by regex '" + item2 + "'' from source '" + text2 + "'");
												text = text.Replace(capture2.Value, Helper.CreateReplaceString(ReplaceCharacters, capture2.Value.Length));
											}
										}
										finally
										{
											IDisposable disposable3;
											if ((disposable3 = (enumerator7 as IDisposable)) != null)
											{
												disposable3.Dispose();
											}
										}
									}
								}
								finally
								{
									IDisposable disposable4;
									if ((disposable4 = (enumerator6 as IDisposable)) != null)
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
				else
				{
					if (sources == null || sources.Length == 0)
					{
						foreach (Regex value4 in domainsRegex.Values)
						{
							MatchCollection matchCollection3 = value4.Matches(testString);
							IEnumerator enumerator9 = matchCollection3.GetEnumerator();
							try
							{
								while (enumerator9.MoveNext())
								{
									Match match3 = (Match)enumerator9.Current;
									IEnumerator enumerator10 = match3.Captures.GetEnumerator();
									try
									{
										while (enumerator10.MoveNext())
										{
											Capture capture3 = (Capture)enumerator10.Current;
											text = text.Replace(capture3.Value, Helper.CreateReplaceString(ReplaceCharacters, capture3.Value.Length));
										}
									}
									finally
									{
										IDisposable disposable5;
										if ((disposable5 = (enumerator10 as IDisposable)) != null)
										{
											disposable5.Dispose();
										}
									}
								}
							}
							finally
							{
								IDisposable disposable6;
								if ((disposable6 = (enumerator9 as IDisposable)) != null)
								{
									disposable6.Dispose();
								}
							}
						}
						return text;
					}
					foreach (string text3 in sources)
					{
						if (domainsRegex.TryGetValue(text3, out Regex value2))
						{
							MatchCollection matchCollection4 = value2.Matches(testString);
							IEnumerator enumerator11 = matchCollection4.GetEnumerator();
							try
							{
								while (enumerator11.MoveNext())
								{
									Match match4 = (Match)enumerator11.Current;
									IEnumerator enumerator12 = match4.Captures.GetEnumerator();
									try
									{
										while (enumerator12.MoveNext())
										{
											Capture capture4 = (Capture)enumerator12.Current;
											text = text.Replace(capture4.Value, Helper.CreateReplaceString(ReplaceCharacters, capture4.Value.Length));
										}
									}
									finally
									{
										IDisposable disposable7;
										if ((disposable7 = (enumerator12 as IDisposable)) != null)
										{
											disposable7.Dispose();
										}
									}
								}
							}
							finally
							{
								IDisposable disposable8;
								if ((disposable8 = (enumerator11 as IDisposable)) != null)
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
			else
			{
				logFilterNotReady();
			}
			return text;
		}

		public override string Replace(string text, List<string> domains)
		{
			string text2 = text;
			if (string.IsNullOrEmpty(text))
			{
				logReplace();
				text2 = string.Empty;
			}
			else
			{
				if (domains != null && domains.Count != 0)
				{
					{
						foreach (string domain in domains)
						{
							text2 = text2.Replace(domain, Helper.CreateReplaceString(ReplaceCharacters, domain.Length));
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
