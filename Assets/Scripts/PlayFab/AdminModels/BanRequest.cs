using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class BanRequest : PlayFabRequestCommon
	{
		public uint? DurationInHours;

		public string IPAddress;

		public string MACAddress;

		public string PlayFabId;

		public string Reason;
	}
}
