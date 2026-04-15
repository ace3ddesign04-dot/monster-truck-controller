using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AuthenticateSessionTicketRequest : PlayFabRequestCommon
	{
		public string SessionTicket;
	}
}
