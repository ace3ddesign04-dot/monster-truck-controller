using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetUserAccountInfoResult : PlayFabResultCommon
	{
		public UserAccountInfo UserInfo;
	}
}
