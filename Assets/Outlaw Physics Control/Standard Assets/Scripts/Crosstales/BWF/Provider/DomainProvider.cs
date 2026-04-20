using Crosstales.BWF.Model;
using Crosstales.BWF.Util;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Crosstales.BWF.Provider
{
	public abstract class DomainProvider : BaseProvider
	{
		protected List<Domains> domains = new List<Domains>();

		private const string domainRegexStart = "\\b{0,1}((ht|f)tp(s?)\\:\\/\\/)?[\\w\\-\\.\\@]*[\\.]";

		private const string domainRegexEnd = "(:\\d{1,5})?(\\/|\\b)";

		private Dictionary<string, Regex> domainsRegex = new Dictionary<string, Regex>();

		private Dictionary<string, List<Regex>> debugDomainsRegex = new Dictionary<string, List<Regex>>();

		public Dictionary<string, Regex> DomainsRegex
		{
			get
			{
				return domainsRegex;
			}
			protected set
			{
				domainsRegex = value;
			}
		}

		public Dictionary<string, List<Regex>> DebugDomainsRegex
		{
			get
			{
				return debugDomainsRegex;
			}
			protected set
			{
				debugDomainsRegex = value;
			}
		}

		public override void Load()
		{
			if (ClearOnLoad)
			{
				domains.Clear();
			}
		}

		protected override void init()
		{
			DomainsRegex.Clear();
			if (Config.DEBUG_DOMAINS)
			{
				UnityEngine.Debug.Log("++ DomainProvider '" + Name + "' started in debug-mode ++");
			}
			foreach (Domains domain in domains)
			{
				if (Config.DEBUG_DOMAINS)
				{
					try
					{
						List<Regex> list = new List<Regex>(domain.DomainList.Count);
						foreach (string domain2 in domain.DomainList)
						{
							list.Add(new Regex("\\b{0,1}((ht|f)tp(s?)\\:\\/\\/)?[\\w\\-\\.\\@]*[\\.]" + domain2 + "(:\\d{1,5})?(\\/|\\b)", RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5));
						}
						if (!DebugDomainsRegex.ContainsKey(domain.Source.Name))
						{
							DebugDomainsRegex.Add(domain.Source.Name, list);
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("Could not generate debug regex for source '" + domain.Source.Name + "': " + ex);
						if (Constants.DEV_DEBUG)
						{
							UnityEngine.Debug.Log(domain.DomainList.CTDump());
						}
					}
				}
				else
				{
					try
					{
						if (!DomainsRegex.ContainsKey(domain.Source.Name))
						{
							DomainsRegex.Add(domain.Source.Name, new Regex("\\b{0,1}((ht|f)tp(s?)\\:\\/\\/)?[\\w\\-\\.\\@]*[\\.](" + string.Join("|", domain.DomainList.ToArray()) + ")(:\\d{1,5})?(\\/|\\b)", RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5));
						}
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogError("Could not generate exact regex for source '" + domain.Source.Name + "': " + ex2);
						if (Constants.DEV_DEBUG)
						{
							UnityEngine.Debug.Log(domain.DomainList.CTDump());
						}
					}
				}
				if (Config.DEBUG_DOMAINS)
				{
					UnityEngine.Debug.Log("Domain resource '" + domain.Source + "' loaded and " + domain.DomainList.Count + " entries found.");
				}
			}
			base.isReady = true;
		}
	}
}
