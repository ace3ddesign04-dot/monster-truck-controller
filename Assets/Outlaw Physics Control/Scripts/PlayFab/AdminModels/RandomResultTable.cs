using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RandomResultTable
	{
		public List<ResultTableNode> Nodes;

		public string TableId;
	}
}
