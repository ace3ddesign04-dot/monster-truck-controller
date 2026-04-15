using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkWindowsHelloAccountRequest : PlayFabRequestCommon
	{
		public string DeviceName;

		public bool? ForceLink;

		public string PublicKey;

		public string UserName;
	}
}
