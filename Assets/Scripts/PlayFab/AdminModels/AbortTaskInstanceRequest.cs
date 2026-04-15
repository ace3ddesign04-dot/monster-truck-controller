using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class AbortTaskInstanceRequest : PlayFabRequestCommon
	{
		public string TaskInstanceId;
	}
}
