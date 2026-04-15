using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdateUserTitleDisplayNameRequest : PlayFabRequestCommon
	{
		public string DisplayName;

		public string PlayFabId;
	}
}
