using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class DeviceInfoRequest : PlayFabRequestCommon
	{
		public Dictionary<string, object> Info;
	}
}
