using UnityEngine;

public class SystemProp : Prop
{
	[Header("System prop parameters")]
	public Transform text;

	public GameObject visualElementsParent;

	private Camera mainCamera;

	public override void Start()
	{
		base.Start();
		mainCamera = Camera.main;
	}

	public override void Update()
	{
		base.Update();
		if (text != null)
		{
			text.transform.rotation = Quaternion.LookRotation(text.transform.position - mainCamera.transform.position);
		}
	}

	public void RemoveEditorGizmos()
	{
		if (visualElementsParent != null)
		{
			UnityEngine.Object.Destroy(visualElementsParent);
		}
	}
}
