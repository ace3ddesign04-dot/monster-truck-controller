using UnityEngine;

namespace Crosstales.BWF.Demo.Util
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_demo_1_1_util_1_1_random_rotator.html")]
	public class RandomRotator : MonoBehaviour
	{
		public Vector3 Speed;

		public Vector2 ChangeInterval = new Vector2(10f, 45f);

		private Transform tf;

		private Vector3 speed;

		private float elapsedTime;

		private float changeTime;

		public void Start()
		{
			tf = base.transform;
			elapsedTime = (changeTime = UnityEngine.Random.Range(ChangeInterval.x, ChangeInterval.y));
		}

		public void Update()
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > changeTime)
			{
				speed.x = UnityEngine.Random.Range(0f - Mathf.Abs(Speed.x), Mathf.Abs(Speed.x));
				speed.y = UnityEngine.Random.Range(0f - Mathf.Abs(Speed.y), Mathf.Abs(Speed.y));
				speed.z = UnityEngine.Random.Range(0f - Mathf.Abs(Speed.z), Mathf.Abs(Speed.z));
				changeTime = UnityEngine.Random.Range(ChangeInterval.x, ChangeInterval.y);
				elapsedTime = 0f;
			}
			tf.Rotate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime);
		}
	}
}
