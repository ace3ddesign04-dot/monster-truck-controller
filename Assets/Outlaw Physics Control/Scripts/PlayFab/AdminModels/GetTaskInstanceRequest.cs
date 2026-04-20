using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetTaskInstanceRequest : PlayFabRequestCommon
	{
		public string TaskInstanceId;
	}
}
