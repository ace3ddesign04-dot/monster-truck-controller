using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UnlinkCustomIDRequest : PlayFabRequestCommon
	{
		public string CustomId;
	}
}
