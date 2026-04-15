using System;

namespace Gaia.FullSerializer
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class fsIgnoreAttribute : Attribute
	{
	}
}
