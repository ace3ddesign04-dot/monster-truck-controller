using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetAccountInfoResult : PlayFabResultCommon
	{
		public UserAccountInfo AccountInfo;
	}
}
