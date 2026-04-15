using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RedeemMatchmakerTicketRequest : PlayFabRequestCommon
	{
		public string LobbyId;

		public string Ticket;
	}
}
