using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AddOrUpdateContactEmailRequest : PlayFabRequestCommon
	{
		public string EmailAddress;
	}
}
