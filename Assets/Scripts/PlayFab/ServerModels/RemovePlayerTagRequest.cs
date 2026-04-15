using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RemovePlayerTagRequest : PlayFabRequestCommon
	{
		public string PlayFabId;

		public string TagName;
	}
}
