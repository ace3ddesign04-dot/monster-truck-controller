using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetAllSegmentsResult : PlayFabResultCommon
	{
		public List<GetSegmentResult> Segments;
	}
}
