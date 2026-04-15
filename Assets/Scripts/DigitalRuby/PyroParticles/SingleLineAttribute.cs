using UnityEngine;

namespace DigitalRuby.PyroParticles
{
	public class SingleLineAttribute : PropertyAttribute
	{
		public string Tooltip
		{
			get;
			private set;
		}

		public SingleLineAttribute(string tooltip)
		{
			Tooltip = tooltip;
		}
	}
}
