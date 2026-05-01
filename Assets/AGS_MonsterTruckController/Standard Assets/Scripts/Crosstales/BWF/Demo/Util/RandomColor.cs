using Crosstales.BWF.Util;
using UnityEngine;

namespace Crosstales.BWF.Demo.Util
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_demo_1_1_util_1_1_random_color.html")]
	public class RandomColor : MonoBehaviour
	{
		public Vector2 ChangeInterval = new Vector2(5f, 15f);

		private float elapsedTime;

		private float changeTime;

		private Renderer currentRenderer;

		private Color32 startColor;

		private Color32 endColor;

		private float lerpTime;

		public void Start()
		{
			currentRenderer = GetComponent<Renderer>();
			elapsedTime = (changeTime = UnityEngine.Random.Range(ChangeInterval.x, ChangeInterval.y));
			startColor = currentRenderer.material.color;
		}

		public void Update()
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > changeTime)
			{
				endColor = Helper.HSVToRGB(UnityEngine.Random.Range(0f, 360f), 1f, 1f);
				changeTime = UnityEngine.Random.Range(ChangeInterval.x, ChangeInterval.y);
				lerpTime = (elapsedTime = 0f);
			}
			currentRenderer.material.color = Color.Lerp(startColor, endColor, lerpTime);
			if (lerpTime < 1f)
			{
				lerpTime += Time.deltaTime / (changeTime - 0.2f);
			}
			else
			{
				startColor = currentRenderer.material.color;
			}
		}
	}
}
