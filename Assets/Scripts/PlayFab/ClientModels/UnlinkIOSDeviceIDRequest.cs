using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UnlinkIOSDeviceIDRequest : PlayFabRequestCommon
	{
		public string DeviceId;
	}
}
