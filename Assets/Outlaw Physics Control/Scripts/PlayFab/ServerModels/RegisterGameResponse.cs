using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RegisterGameResponse : PlayFabResultCommon
	{
		public string LobbyId;
	}
}
