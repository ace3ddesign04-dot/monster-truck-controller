using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ScheduledTask
	{
		public string Description;

		public bool IsActive;

		public DateTime? LastRunTime;

		public string Name;

		public DateTime? NextRunTime;

		public object Parameter;

		public string Schedule;

		public string TaskId;

		public ScheduledTaskType? Type;
	}
}
