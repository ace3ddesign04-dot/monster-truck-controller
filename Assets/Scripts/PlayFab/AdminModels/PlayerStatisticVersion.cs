using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class PlayerStatisticVersion
	{
		public DateTime ActivationTime;

		public string ArchiveDownloadUrl;

		public DateTime? DeactivationTime;

		public DateTime? ScheduledActivationTime;

		public DateTime? ScheduledDeactivationTime;

		public string StatisticName;

		public StatisticVersionStatus? Status;

		public uint Version;
	}
}
