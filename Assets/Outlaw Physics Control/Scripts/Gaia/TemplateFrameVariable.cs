using System.Collections.Generic;

namespace Gaia
{
	public class TemplateFrameVariable
	{
		public List<int> Indicies;

		public List<int> Positions;

		public TemplateFrameVariable(List<int> indicies, List<int> positions)
		{
			Indicies = indicies;
			Positions = positions;
		}
	}
}
