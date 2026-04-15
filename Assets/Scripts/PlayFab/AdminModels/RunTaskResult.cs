using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RunTaskResult : PlayFabResultCommon
	{
		public string TaskInstanceId;
	}
}
