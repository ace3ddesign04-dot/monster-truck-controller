using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetTaskInstancesResult : PlayFabResultCommon
	{
		public List<TaskInstanceBasicSummary> Summaries;
	}
}
