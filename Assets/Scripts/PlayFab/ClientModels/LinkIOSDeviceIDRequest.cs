using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkIOSDeviceIDRequest : PlayFabRequestCommon
	{
		public string DeviceId;

		public string DeviceModel;

		public bool? ForceLink;

		public string OS;
	}
}
