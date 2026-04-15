using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetTasksResult : PlayFabResultCommon
	{
		public List<ScheduledTask> Tasks;
	}
}
