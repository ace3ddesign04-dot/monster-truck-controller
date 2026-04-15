using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UserAccountInfo
	{
		public UserAndroidDeviceInfo AndroidDeviceInfo;

		public DateTime Created;

		public UserCustomIdInfo CustomIdInfo;

		public UserFacebookInfo FacebookInfo;

		public UserGameCenterInfo GameCenterInfo;

		public UserGoogleInfo GoogleInfo;

		public UserIosDeviceInfo IosDeviceInfo;

		public UserKongregateInfo KongregateInfo;

		public string PlayFabId;

		public UserPrivateAccountInfo PrivateInfo;

		public UserPsnInfo PsnInfo;

		public UserSteamInfo SteamInfo;

		public UserTitleInfo TitleInfo;

		public UserTwitchInfo TwitchInfo;

		public string Username;

		public UserXboxInfo XboxInfo;
	}
}
