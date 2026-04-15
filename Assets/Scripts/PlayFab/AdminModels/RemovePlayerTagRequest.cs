using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RemovePlayerTagRequest : PlayFabRequestCommon
	{
		public string PlayFabId;

		public string TagName;
	}
}
