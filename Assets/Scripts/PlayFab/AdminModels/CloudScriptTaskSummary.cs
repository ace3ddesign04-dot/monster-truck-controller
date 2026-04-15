using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CloudScriptTaskSummary
	{
		public DateTime? CompletedAt;

		public double? EstimatedSecondsRemaining;

		public double? PercentComplete;

		public ExecuteCloudScriptResult Result;

		public string ScheduledByUserId;

		public DateTime StartedAt;

		public TaskInstanceStatus? Status;

		public NameIdentifier TaskIdentifier;

		public string TaskInstanceId;
	}
}
