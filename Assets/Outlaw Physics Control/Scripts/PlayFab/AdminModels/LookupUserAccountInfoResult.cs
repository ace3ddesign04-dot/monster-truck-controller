using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class LookupUserAccountInfoResult : PlayFabResultCommon
	{
		public UserAccountInfo UserInfo;
	}
}
