using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class WriteEventResponse : PlayFabResultCommon
	{
		public string EventId;
	}
}
