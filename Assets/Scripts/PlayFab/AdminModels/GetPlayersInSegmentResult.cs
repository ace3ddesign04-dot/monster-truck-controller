using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayersInSegmentResult : PlayFabResultCommon
	{
		public string ContinuationToken;

		public List<PlayerProfile> PlayerProfiles;

		public int ProfilesInSegment;
	}
}
