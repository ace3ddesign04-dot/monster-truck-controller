using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CloudScriptVersionStatus
	{
		public int LatestRevision;

		public int PublishedRevision;

		public int Version;
	}
}
