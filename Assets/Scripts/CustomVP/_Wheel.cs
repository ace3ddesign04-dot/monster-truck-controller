using System;
using UnityEngine;

namespace CustomVP
{
	[Serializable]
	public class _Wheel
	{
		public WheelComponent wc;

		[HideInInspector]
		public bool steer;

		[HideInInspector]
		public bool inverseSteer;

		[HideInInspector]
		public bool power;

		[HideInInspector]
		public bool handbrake;
	}
}
