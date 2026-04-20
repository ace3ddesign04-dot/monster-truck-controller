using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UpdateUserTitleDisplayNameRequest : PlayFabRequestCommon
	{
		public string DisplayName;
	}
}
