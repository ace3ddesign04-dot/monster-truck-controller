using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class StatisticValue
	{
		public string StatisticName;

		public int Value;

		public uint Version;
	}
}
