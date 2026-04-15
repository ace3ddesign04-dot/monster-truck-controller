using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetActionsOnPlayersInSegmentTaskInstanceResult : PlayFabResultCommon
	{
		public ActionsOnPlayersInSegmentTaskParameter Parameter;

		public ActionsOnPlayersInSegmentTaskSummary Summary;
	}
}
