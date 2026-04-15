using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class AdCampaignAttributionModel
	{
		public DateTime AttributedAt;

		public string CampaignId;

		public string Platform;
	}
}
