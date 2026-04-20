using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class NotifyMatchmakerPlayerLeftResult : PlayFabResultCommon
	{
		public PlayerConnectionState? PlayerState;
	}
}
