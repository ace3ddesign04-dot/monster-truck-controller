using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CreateActionsOnPlayerSegmentTaskRequest : PlayFabRequestCommon
	{
		public string Description;

		public bool IsActive;

		public string Name;

		public ActionsOnPlayersInSegmentTaskParameter Parameter;

		public string Schedule;
	}
}
