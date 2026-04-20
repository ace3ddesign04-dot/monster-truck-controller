using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class TaskInstanceBasicSummary
	{
		public DateTime? CompletedAt;

		public double? EstimatedSecondsRemaining;

		public double? PercentComplete;

		public string ScheduledByUserId;

		public DateTime StartedAt;

		public TaskInstanceStatus? Status;

		public NameIdentifier TaskIdentifier;

		public string TaskInstanceId;

		public ScheduledTaskType? Type;
	}
}
