using UnityEngine;

namespace CustomVP
{
	public class CustomWheelFrictionCurve
	{
		private AnimationCurve curveData;

		private float extSlip;

		private float extVal;

		private float asSlip;

		private float asVal;

		private float tailVal;

		private Keyframe[] keyframes;

		public float extremumSlip
		{
			get
			{
				return extSlip;
			}
			set
			{
				extSlip = value;
				setupCurve();
			}
		}

		public float extremumValue
		{
			get
			{
				return extVal;
			}
			set
			{
				extVal = value;
				setupCurve();
			}
		}

		public float asymptoteSlip
		{
			get
			{
				return asSlip;
			}
			set
			{
				asSlip = value;
				setupCurve();
			}
		}

		public float asymptoteValue
		{
			get
			{
				return asVal;
			}
			set
			{
				asVal = value;
				setupCurve();
			}
		}

		public float tailValue
		{
			get
			{
				return tailVal;
			}
			set
			{
				tailVal = value;
				setupCurve();
			}
		}

		public float max => Mathf.Max(asVal, extVal);

		public CustomWheelFrictionCurve()
			: this(0.06f, 1.2f, 0.08f, 1f, 0.6f)
		{
		}

		public CustomWheelFrictionCurve(float extSlip, float extVal, float asSlip, float asVal, float tailVal)
		{
			keyframes = new Keyframe[4];
			curveData = new AnimationCurve();
			this.extSlip = extSlip;
			this.extVal = extVal;
			this.asSlip = asSlip;
			this.asVal = asVal;
			this.tailVal = tailVal;
			setupCurve();
		}

		public float evaluate(float slipRatio)
		{
			return curveData.Evaluate(clampRatio(slipRatio));
		}

		private void setupCurve()
		{
			keyframes[0].time = 0f;
			keyframes[0].value = 0f;
			keyframes[1].time = extSlip;
			keyframes[1].value = extVal;
			keyframes[2].time = asSlip;
			keyframes[2].value = asVal;
			keyframes[3].time = 1f;
			keyframes[3].value = tailVal;
			int length = curveData.length;
			for (int num = length - 1; num >= 0; num--)
			{
				curveData.RemoveKey(num);
			}
			curveData.AddKey(keyframes[0]);
			curveData.AddKey(keyframes[1]);
			curveData.AddKey(keyframes[2]);
			curveData.AddKey(keyframes[3]);
		}

		private float clampRatio(float slipRatio)
		{
			slipRatio = Mathf.Abs(slipRatio);
			slipRatio = Mathf.Min(1f, slipRatio);
			slipRatio = Mathf.Max(0f, slipRatio);
			return slipRatio;
		}
	}
}
