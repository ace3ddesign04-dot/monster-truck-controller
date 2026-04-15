using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UpdateAvatarUrlRequest : PlayFabRequestCommon
	{
		public string ImageUrl;
	}
}
