using UnityEngine;

namespace Crosstales.UI
{
	public class WindowManager : MonoBehaviour
	{
		[Tooltip("Window movement speed (default: 3).")]
		public float Speed = 3f;

		private UIFocus Focus;

		private bool open;

		private bool close;

		private Vector3 startPos;

		private Vector3 centerPos;

		private Vector3 lerpPos;

		private float openProgress;

		private float closeProgress;

		private GameObject panel;

		public void Start()
		{
			panel = base.transform.Find("Panel").gameObject;
			startPos = base.transform.position;
			ClosePanel();
		}

		public void Update()
		{
			centerPos = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
			if (open && openProgress < 1f)
			{
				openProgress += Speed * Time.deltaTime;
				base.transform.position = Vector3.Lerp(lerpPos, centerPos, openProgress);
			}
			else if (close)
			{
				if (closeProgress < 1f)
				{
					closeProgress += Speed * Time.deltaTime;
					base.transform.position = Vector3.Lerp(lerpPos, startPos, closeProgress);
				}
				else
				{
					panel.SetActive(value: false);
				}
			}
		}

		public void SwitchPanel()
		{
			if (open)
			{
				ClosePanel();
			}
			else
			{
				OpenPanel();
			}
		}

		public void OpenPanel()
		{
			panel.SetActive(value: true);
			Focus = base.gameObject.GetComponent<UIFocus>();
			Focus.onPanelEnter();
			lerpPos = base.transform.position;
			open = true;
			close = false;
			openProgress = 0f;
		}

		public void ClosePanel()
		{
			lerpPos = base.transform.position;
			open = false;
			close = true;
			closeProgress = 0f;
		}
	}
}
