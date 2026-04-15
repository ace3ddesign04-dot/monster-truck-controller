using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTitleNewsRequest : PlayFabRequestCommon
	{
		public int? Count;
	}
}
