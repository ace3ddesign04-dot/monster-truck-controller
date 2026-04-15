using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ResultTableNode
	{
		public string ResultItem;

		public ResultTableNodeType ResultItemType;

		public int Weight;
	}
}
