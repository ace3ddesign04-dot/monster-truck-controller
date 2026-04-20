using UnityEngine;

namespace MeshCombineStudio
{
	[ExecuteInEditMode]
	public class GarbageCollectMesh : MonoBehaviour
	{
		public Mesh mesh;

		private void Awake()
		{
			MeshFilter component = GetComponent<MeshFilter>();
			if (component != null)
			{
				mesh = component.sharedMesh;
			}
			else
			{
				UnityEngine.Debug.Log("MeshFilter = null");
			}
		}

		private void OnDestroy()
		{
			if (mesh != null)
			{
				UnityEngine.Object.Destroy(mesh);
			}
		}
	}
}
