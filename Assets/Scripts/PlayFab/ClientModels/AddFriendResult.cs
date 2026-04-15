using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AddFriendResult : PlayFabResultCommon
	{
		public bool Created;
	}
}
