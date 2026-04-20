using PlayFab.SharedModels;
using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class RegisterGameResponse : PlayFabResultCommon
	{
		public string LobbyId;
	}
}
