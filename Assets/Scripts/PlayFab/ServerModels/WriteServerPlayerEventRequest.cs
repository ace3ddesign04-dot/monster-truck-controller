using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class WriteServerPlayerEventRequest : PlayFabRequestCommon
	{
		public Dictionary<string, object> Body;

		public string EventName;

		public string PlayFabId;

		public DateTime? Timestamp;
	}
}
