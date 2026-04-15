using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UserTitleInfo
	{
		public string AvatarUrl;

		public DateTime Created;

		public string DisplayName;

		public DateTime? FirstLogin;

		public bool? isBanned;

		public DateTime? LastLogin;

		public UserOrigination? Origination;
	}
}
