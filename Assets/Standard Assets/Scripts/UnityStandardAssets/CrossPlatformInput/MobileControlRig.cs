using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	[ExecuteInEditMode]
	public class MobileControlRig : MonoBehaviour
	{
		private void OnEnable()
		{
			CheckEnableControlRig();
		}

		private void Start()
		{
			EventSystem x = UnityEngine.Object.FindObjectOfType<EventSystem>();
			if (x == null)
			{
				GameObject gameObject = new GameObject("EventSystem");
				gameObject.AddComponent<EventSystem>();
				gameObject.AddComponent<StandaloneInputModule>();
			}
		}

		private void CheckEnableControlRig()
		{
			EnableControlRig(enabled: true);
		}

		private void EnableControlRig(bool enabled)
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					transform.gameObject.SetActive(enabled);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
