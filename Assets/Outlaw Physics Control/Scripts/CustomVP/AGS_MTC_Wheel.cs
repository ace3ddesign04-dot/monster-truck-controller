using System;
using UnityEngine;

namespace AGS_MonsterTruckControl
{
	[Serializable]
	public class AGS_MTC_Wheel
	{
		public AGS_MTC_WheelComponent wc;

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
