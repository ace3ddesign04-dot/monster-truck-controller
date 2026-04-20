using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ModifyServerBuildResult : PlayFabResultCommon
	{
		public List<Region> ActiveRegions;

		public string BuildId;

		public string CommandLineTemplate;

		public string Comment;

		public string ExecutablePath;

		public int MaxGamesPerHost;

		public int MinFreeGameSlots;

		public GameBuildStatus? Status;

		public DateTime Timestamp;

		public string TitleId;
	}
}
