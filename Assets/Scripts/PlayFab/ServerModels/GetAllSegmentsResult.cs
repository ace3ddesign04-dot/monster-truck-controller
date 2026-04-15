using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetAllSegmentsResult : PlayFabResultCommon
	{
		public List<GetSegmentResult> Segments;
	}
}
