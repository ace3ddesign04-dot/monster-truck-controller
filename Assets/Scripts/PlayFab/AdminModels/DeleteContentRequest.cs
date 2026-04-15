using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class DeleteContentRequest : PlayFabRequestCommon
	{
		public string Key;
	}
}
