using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class CreateSharedGroupRequest : PlayFabRequestCommon
	{
		public string SharedGroupId;
	}
}
