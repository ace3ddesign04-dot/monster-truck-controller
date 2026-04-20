using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class CreateSharedGroupResult : PlayFabResultCommon
	{
		public string SharedGroupId;
	}
}
