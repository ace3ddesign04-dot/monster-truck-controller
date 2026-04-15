using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdateUserTitleDisplayNameResult : PlayFabResultCommon
	{
		public string DisplayName;
	}
}
