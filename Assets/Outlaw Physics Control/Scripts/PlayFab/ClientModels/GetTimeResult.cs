using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTimeResult : PlayFabResultCommon
	{
		public DateTime Time;
	}
}
