using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerSegmentsResult : PlayFabResultCommon
	{
		public List<GetSegmentResult> Segments;
	}
}
