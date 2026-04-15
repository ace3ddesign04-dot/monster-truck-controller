namespace Crosstales.BWF.Util
{
	public static class Config
	{
		public static bool DEBUG;

		public static bool DEBUG_BADWORDS;

		public static bool DEBUG_DOMAINS;

		public static bool isLoaded;

		public static void Reset()
		{
			DEBUG = false;
			DEBUG_BADWORDS = false;
			DEBUG_DOMAINS = false;
		}

		public static void Load()
		{
			if (CTPlayerPrefs.HasKey("BWF_CFG_DEBUG"))
			{
				DEBUG = CTPlayerPrefs.GetBool("BWF_CFG_DEBUG");
			}
			if (CTPlayerPrefs.HasKey("BWF_CFG_DEBUG_BADWORDS"))
			{
				DEBUG_BADWORDS = CTPlayerPrefs.GetBool("BWF_CFG_DEBUG_BADWORDS");
			}
			if (CTPlayerPrefs.HasKey("BWF_CFG_DEBUG_DOMAINS"))
			{
				DEBUG_DOMAINS = CTPlayerPrefs.GetBool("BWF_CFG_DEBUG_DOMAINS");
			}
			isLoaded = true;
		}

		public static void Save()
		{
			CTPlayerPrefs.SetBool("BWF_CFG_DEBUG", DEBUG);
			CTPlayerPrefs.SetBool("BWF_CFG_DEBUG_BADWORDS", DEBUG_BADWORDS);
			CTPlayerPrefs.SetBool("BWF_CFG_DEBUG_DOMAINS", DEBUG_DOMAINS);
			CTPlayerPrefs.Save();
		}
	}
}
