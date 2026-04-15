using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerSegmentsResult : PlayFabResultCommon
	{
		public List<GetSegmentResult> Segments;
	}
}
