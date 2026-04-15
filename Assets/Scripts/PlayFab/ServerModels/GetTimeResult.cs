using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetTimeResult : PlayFabResultCommon
	{
		public DateTime Time;
	}
}
