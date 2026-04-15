using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class AddPlayerTagRequest : PlayFabRequestCommon
	{
		public string PlayFabId;

		public string TagName;
	}
}
