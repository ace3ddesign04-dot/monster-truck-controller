using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AdCampaignAttribution
	{
		public DateTime AttributedAt;

		public string CampaignId;

		public string Platform;
	}
}
