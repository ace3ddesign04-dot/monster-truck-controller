using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI.Util
{
	public class FPSDisplay : MonoBehaviour
	{
		public Text FPS;

		private float deltaTime;

		private float elapsedTime;

		private float msec;

		private float fps;

		private string text;

		public void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1f)
			{
				if (Time.frameCount % 3 == 0 && FPS != null)
				{
					msec = deltaTime * 1000f;
					fps = 1f / deltaTime;
					text = $"<b>FPS: {fps:0.}</b> ({msec:0.0} ms)";
					if (fps < 15f)
					{
						FPS.text = "<color=red>" + text + "</color>";
					}
					else if (fps < 29f)
					{
						FPS.text = "<color=orange>" + text + "</color>";
					}
					else
					{
						FPS.text = "<color=green>" + text + "</color>";
					}
				}
			}
			else
			{
				FPS.text = "<i>...calculating <b>FPS</b>...</i>";
			}
		}
	}
}
