using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class ButtonHandler : MonoBehaviour
	{
		private Button m_Button;

		public string Name;

		private void OnEnable()
		{
			m_Button = GetComponent<Button>();
		}

		public void SetDownState()
		{
			if (!(m_Button != null) || m_Button.interactable)
			{
				CrossPlatformInputManager.SetButtonDown(Name);
			}
		}

		public void SetUpState()
		{
			if (!(m_Button != null) || m_Button.interactable)
			{
				CrossPlatformInputManager.SetButtonUp(Name);
			}
		}

		public void SetAxisPositiveState()
		{
			CrossPlatformInputManager.SetAxisPositive(Name);
		}

		public void SetAxisNeutralState()
		{
			CrossPlatformInputManager.SetAxisZero(Name);
		}

		public void SetAxisNegativeState()
		{
			CrossPlatformInputManager.SetAxisNegative(Name);
		}

		public void Update()
		{
		}
	}
}
