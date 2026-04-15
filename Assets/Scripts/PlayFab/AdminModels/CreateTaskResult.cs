using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CreateTaskResult : PlayFabResultCommon
	{
		public string TaskId;
	}
}
