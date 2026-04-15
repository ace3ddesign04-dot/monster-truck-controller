using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetContentListResult : PlayFabResultCommon
	{
		public List<ContentInfo> Contents;

		public int ItemCount;

		public uint TotalSize;
	}
}
