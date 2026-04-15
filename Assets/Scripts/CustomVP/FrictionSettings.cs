using System;
using UnityEngine;

namespace CustomVP
{
	[Serializable]
	public class FrictionSettings
	{
		[Header("Forward friction")]
		public float f_ExtremumSlip = 0.06f;

		public float f_ExtremumValue = 1.2f;

		public float f_AsymptoteSlip = 0.065f;

		public float f_AsymptoteValue = 1.25f;

		public float f_TailValue = 0.7f;

		[Header("Side friction")]
		public float s_ExtremumSlip = 0.03f;

		public float s_ExtremumValue = 1f;

		public float s_AsymptoteSlip = 0.04f;

		public float s_AsymptoteValue = 1.05f;

		public float s_TailValue = 0.7f;
	}
}
