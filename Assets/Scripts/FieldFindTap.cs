using UnityEngine;

public class FieldFindTap : MonoBehaviour
{
	private MenuManager menuManager;

	private void Start()
	{
		menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
	}

	private void OnMouseDown()
	{
		if (menuManager != null)
		{
			menuManager.ShowFieldFindMessage();
		}
	}
}
