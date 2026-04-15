using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkKongregateAccountRequest : PlayFabRequestCommon
	{
		public string AuthTicket;

		public bool? ForceLink;

		public string KongregateId;
	}
}
