using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class AdCampaignAttribution
	{
		public DateTime AttributedAt;

		public string CampaignId;

		public string Platform;
	}
}
