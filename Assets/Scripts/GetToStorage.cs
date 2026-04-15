using UnityEngine;

public class GetToStorage : MonoBehaviour
{
	private MenuManager menuManager;

	private void Start()
	{
		menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
	}

	private void Update()
	{
	}

	private void OnMouseDown()
	{
		menuManager.ShowStorage();
	}
}
