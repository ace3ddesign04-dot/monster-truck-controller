using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetCloudScriptRevisionResult : PlayFabResultCommon
	{
		public DateTime CreatedAt;

		public List<CloudScriptFile> Files;

		public bool IsPublished;

		public int Revision;

		public int Version;
	}
}
