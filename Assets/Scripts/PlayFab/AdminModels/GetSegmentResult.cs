using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetSegmentResult : PlayFabResultCommon
	{
		public string ABTestParent;

		public string Id;

		public string Name;
	}
}
