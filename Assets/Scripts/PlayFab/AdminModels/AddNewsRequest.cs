using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class AddNewsRequest : PlayFabRequestCommon
	{
		public string Body;

		public DateTime? Timestamp;

		public string Title;
	}
}
