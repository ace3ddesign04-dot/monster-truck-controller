using UnityEngine;

[ExecuteInEditMode]
public class ListLODGroups : MonoBehaviour
{
	private void Start()
	{
		LODGroup[] componentsInChildren = GetComponentsInChildren<LODGroup>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Debug.Log(componentsInChildren[i].name);
		}
		UnityEngine.Debug.Log("-----------------------------------------------");
		UnityEngine.Debug.Log(componentsInChildren.Length + " LodGroups found");
	}
}
