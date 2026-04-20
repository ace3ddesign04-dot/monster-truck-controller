using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class WriteEventResponse : PlayFabResultCommon
	{
		public string EventId;
	}
}
