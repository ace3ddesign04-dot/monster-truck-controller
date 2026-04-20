using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AddPlayerTagRequest : PlayFabRequestCommon
	{
		public string PlayFabId;

		public string TagName;
	}
}
