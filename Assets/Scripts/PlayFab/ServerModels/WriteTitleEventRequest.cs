using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class WriteTitleEventRequest : PlayFabRequestCommon
	{
		public Dictionary<string, object> Body;

		public string EventName;

		public DateTime? Timestamp;
	}
}
