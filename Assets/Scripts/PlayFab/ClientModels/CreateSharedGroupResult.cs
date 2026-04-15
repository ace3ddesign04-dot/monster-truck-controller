using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class CreateSharedGroupResult : PlayFabResultCommon
	{
		public string SharedGroupId;
	}
}
