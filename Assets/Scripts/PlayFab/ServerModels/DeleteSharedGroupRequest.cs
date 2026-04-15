using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class DeleteSharedGroupRequest : PlayFabRequestCommon
	{
		public string SharedGroupId;
	}
}
