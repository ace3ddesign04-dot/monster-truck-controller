using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetCloudScriptTaskInstanceResult : PlayFabResultCommon
	{
		public CloudScriptTaskParameter Parameter;

		public CloudScriptTaskSummary Summary;
	}
}
