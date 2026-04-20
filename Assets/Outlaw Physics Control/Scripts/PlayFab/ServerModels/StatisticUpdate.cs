using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class StatisticUpdate
	{
		public string StatisticName;

		public int Value;

		public uint? Version;
	}
}
