using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ResultTableNode
	{
		public string ResultItem;

		public ResultTableNodeType ResultItemType;

		public int Weight;
	}
}
