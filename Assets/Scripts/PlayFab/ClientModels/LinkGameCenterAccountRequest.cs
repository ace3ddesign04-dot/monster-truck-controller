using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkGameCenterAccountRequest : PlayFabRequestCommon
	{
		public bool? ForceLink;

		public string GameCenterId;
	}
}
