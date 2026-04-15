using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetTaskInstancesRequest : PlayFabRequestCommon
	{
		public DateTime? StartedAtRangeFrom;

		public DateTime? StartedAtRangeTo;

		public TaskInstanceStatus? StatusFilter;

		public NameIdentifier TaskIdentifier;
	}
}
