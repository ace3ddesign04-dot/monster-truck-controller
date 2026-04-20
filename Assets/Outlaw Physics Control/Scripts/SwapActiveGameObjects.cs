using UnityEngine;

public class SwapActiveGameObjects : MonoBehaviour
{
	public GameObject go1;

	public GameObject go2;

	public KeyCode key;

	private bool active = true;

	private void Start()
	{
		SetActive(active);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(key))
		{
			active = !active;
			SetActive(active);
		}
	}

	private void OnGUI()
	{
		GUI.color = Color.red;
		GUI.Label(new Rect(10f, 10f, 200f, 20f), "Toggle with '" + key.ToString() + "' key.");
		if (active)
		{
			GUI.Label(new Rect(10f, 50f, 300f, 20f), "MeshCombineStudio is Enabled.");
		}
		else
		{
			GUI.Label(new Rect(10f, 50f, 300f, 20f), "MeshCombineStudio is Disabled.");
		}
		GUI.color = Color.white;
	}

	private void SetActive(bool active)
	{
		go1.SetActive(active);
		go2.SetActive(!active);
	}
}
