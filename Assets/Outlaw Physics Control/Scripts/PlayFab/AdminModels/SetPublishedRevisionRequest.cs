using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SetPublishedRevisionRequest : PlayFabRequestCommon
	{
		public int Revision;

		public int Version;
	}
}
