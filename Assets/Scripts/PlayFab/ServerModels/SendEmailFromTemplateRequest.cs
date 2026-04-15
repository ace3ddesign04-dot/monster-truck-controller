using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SendEmailFromTemplateRequest : PlayFabRequestCommon
	{
		public string EmailTemplateId;

		public string PlayFabId;
	}
}
