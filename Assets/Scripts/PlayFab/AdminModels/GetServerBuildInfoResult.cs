using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetServerBuildInfoResult : PlayFabResultCommon
	{
		public List<Region> ActiveRegions;

		public string BuildId;

		public string Comment;

		public string ErrorMessage;

		public int MaxGamesPerHost;

		public int MinFreeGameSlots;

		public GameBuildStatus? Status;

		public DateTime Timestamp;

		public string TitleId;
	}
}
