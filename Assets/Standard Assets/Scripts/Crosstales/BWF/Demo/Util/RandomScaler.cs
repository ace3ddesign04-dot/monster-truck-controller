using UnityEngine;

namespace Crosstales.BWF.Demo.Util
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_demo_1_1_util_1_1_random_scaler.html")]
	public class RandomScaler : MonoBehaviour
	{
		public Vector3 ScaleMin = Vector3.zero;

		public Vector3 ScaleMax = Vector3.one;

		public bool Uniform;

		public Vector2 ChangeInterval = new Vector2(10f, 45f);

		private Transform tf;

		private Vector3 endScale;

		private float elapsedTime;

		private float changeTime;

		private Vector3 startScale;

		private float lerpTime;

		public void Start()
		{
			tf = base.transform;
			elapsedTime = (changeTime = UnityEngine.Random.Range(ChangeInterval.x, ChangeInterval.y));
			startScale = tf.localScale;
		}

		public void Update()
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > changeTime)
			{
				if (Uniform)
				{
					endScale.x = (endScale.y = (endScale.z = UnityEngine.Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x))));
				}
				else
				{
					endScale.x = UnityEngine.Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x));
					endScale.y = UnityEngine.Random.Range(ScaleMin.y, Mathf.Abs(ScaleMax.y));
					endScale.z = UnityEngine.Random.Range(ScaleMin.z, Mathf.Abs(ScaleMax.z));
				}
				changeTime = UnityEngine.Random.Range(ChangeInterval.x, ChangeInterval.y);
				lerpTime = (elapsedTime = 0f);
			}
			tf.localScale = Vector3.Lerp(startScale, endScale, lerpTime);
			if (lerpTime < 1f)
			{
				lerpTime += Time.deltaTime / (changeTime - 0.2f);
			}
			else
			{
				startScale = tf.localScale;
			}
		}
	}
}
