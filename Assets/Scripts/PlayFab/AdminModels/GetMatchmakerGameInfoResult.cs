using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetMatchmakerGameInfoResult : PlayFabResultCommon
	{
		public string BuildVersion;

		public DateTime? EndTime;

		public string LobbyId;

		public string Mode;

		public List<string> Players;

		public Region? Region;

		public string ServerAddress;

		public uint ServerPort;

		public DateTime StartTime;

		public string TitleId;
	}
}
