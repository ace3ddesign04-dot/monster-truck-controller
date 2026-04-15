using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI
{
	public class UIFocus : MonoBehaviour
	{
		public string CanvasName = "Canvas";

		private UIWindowManager manager;

		private Image image;

		public void Start()
		{
			manager = GameObject.Find(CanvasName).GetComponent<UIWindowManager>();
			image = base.transform.Find("Panel/Header").GetComponent<Image>();
		}

		public void onPanelEnter()
		{
			manager.ChangeState(base.gameObject);
			Color color = image.color;
			color.a = 255f;
			image.color = color;
			base.transform.SetAsLastSibling();
			base.transform.SetAsFirstSibling();
			base.transform.SetSiblingIndex(-1);
			base.transform.GetSiblingIndex();
		}
	}
}
