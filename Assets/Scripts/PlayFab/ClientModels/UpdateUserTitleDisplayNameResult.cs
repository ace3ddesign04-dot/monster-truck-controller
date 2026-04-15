using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UpdateUserTitleDisplayNameResult : PlayFabResultCommon
	{
		public string DisplayName;
	}
}
