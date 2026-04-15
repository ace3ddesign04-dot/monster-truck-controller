using System;

namespace Gaia.FullSerializer
{
	public abstract class fsConverter : fsBaseConverter
	{
		public abstract bool CanProcess(Type type);
	}
}
