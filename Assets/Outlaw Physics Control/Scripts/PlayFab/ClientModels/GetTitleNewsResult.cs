using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTitleNewsResult : PlayFabResultCommon
	{
		public List<TitleNewsItem> News;
	}
}
