using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class BanInfo
	{
		public bool Active;

		public string BanId;

		public DateTime? Created;

		public DateTime? Expires;

		public string IPAddress;

		public string MACAddress;

		public string PlayFabId;

		public string Reason;
	}
}
