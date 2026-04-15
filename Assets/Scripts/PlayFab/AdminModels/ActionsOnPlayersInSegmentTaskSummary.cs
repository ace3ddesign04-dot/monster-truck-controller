using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ActionsOnPlayersInSegmentTaskSummary
	{
		public DateTime? CompletedAt;

		public string ErrorMessage;

		public bool? ErrorWasFatal;

		public double? EstimatedSecondsRemaining;

		public double? PercentComplete;

		public string ScheduledByUserId;

		public DateTime StartedAt;

		public TaskInstanceStatus? Status;

		public NameIdentifier TaskIdentifier;

		public string TaskInstanceId;

		public int? TotalPlayersInSegment;

		public int? TotalPlayersProcessed;
	}
}
