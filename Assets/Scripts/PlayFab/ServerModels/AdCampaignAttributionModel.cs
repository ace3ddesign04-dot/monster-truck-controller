using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AdCampaignAttributionModel
	{
		public DateTime AttributedAt;

		public string CampaignId;

		public string Platform;
	}
}
