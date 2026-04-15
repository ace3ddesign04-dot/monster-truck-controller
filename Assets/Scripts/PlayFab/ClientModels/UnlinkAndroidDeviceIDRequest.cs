using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UnlinkAndroidDeviceIDRequest : PlayFabRequestCommon
	{
		public string AndroidDeviceId;
	}
}
