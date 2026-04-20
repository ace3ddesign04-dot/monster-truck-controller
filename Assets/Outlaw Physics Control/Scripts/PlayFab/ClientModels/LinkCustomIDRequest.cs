using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkCustomIDRequest : PlayFabRequestCommon
	{
		public string CustomId;

		public bool? ForceLink;
	}
}
