using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetTitleNewsRequest : PlayFabRequestCommon
	{
		public int? Count;
	}
}
