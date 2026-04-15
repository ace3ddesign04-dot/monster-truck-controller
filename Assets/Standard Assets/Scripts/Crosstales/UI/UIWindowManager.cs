using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI
{
	public class UIWindowManager : MonoBehaviour
	{
		[Tooltip("All Windows of the scene.")]
		public GameObject[] Windows;

		private Image image;

		private GameObject DontTouch;

		public void Start()
		{
			GameObject[] windows = Windows;
			foreach (GameObject gameObject in windows)
			{
				image = gameObject.transform.Find("Panel/Header").GetComponent<Image>();
				Color color = image.color;
				color.a = 0.2f;
				image.color = color;
			}
		}

		public void ChangeState(GameObject x)
		{
			GameObject[] windows = Windows;
			foreach (GameObject gameObject in windows)
			{
				if (gameObject != x)
				{
					image = gameObject.transform.Find("Panel/Header").GetComponent<Image>();
					Color color = image.color;
					color.a = 0.2f;
					image.color = color;
				}
				DontTouch = gameObject.transform.Find("Panel/DontTouch").gameObject;
				DontTouch.SetActive(gameObject != x);
			}
		}
	}
}
