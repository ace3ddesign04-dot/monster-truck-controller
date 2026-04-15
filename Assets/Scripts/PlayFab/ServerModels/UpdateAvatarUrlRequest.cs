using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UpdateAvatarUrlRequest : PlayFabRequestCommon
	{
		public string ImageUrl;

		public string PlayFabId;
	}
}
