using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdateCloudScriptResult : PlayFabResultCommon
	{
		public int Revision;

		public int Version;
	}
}
