using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayersInSegmentRequest : PlayFabRequestCommon
	{
		public string ContinuationToken;

		public uint? MaxBatchSize;

		public uint? SecondsToLive;

		public string SegmentId;
	}
}
