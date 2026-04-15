using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class CreateSharedGroupRequest : PlayFabRequestCommon
	{
		public string SharedGroupId;
	}
}
