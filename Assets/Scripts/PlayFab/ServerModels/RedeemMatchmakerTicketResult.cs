using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RedeemMatchmakerTicketResult : PlayFabResultCommon
	{
		public string Error;

		public bool TicketIsValid;

		public UserAccountInfo UserInfo;
	}
}
