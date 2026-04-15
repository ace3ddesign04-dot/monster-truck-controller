using UnityEngine;

namespace MeshCombineStudio
{
	[ExecuteInEditMode]
	public class ChildCount : MonoBehaviour
	{
		public bool lodGroupsActive;

		public bool setLodGroups;

		private void Awake()
		{
			Transform[] componentsInChildren = base.transform.GetComponentsInChildren<Transform>();
			MeshFilter[] componentsInChildren2 = base.transform.GetComponentsInChildren<MeshFilter>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				Mesh sharedMesh = componentsInChildren2[i].sharedMesh;
				if (!(sharedMesh == null))
				{
					num += sharedMesh.vertexCount;
					num2 += sharedMesh.triangles.Length;
				}
			}
			LODGroup[] componentsInChildren3 = base.transform.GetComponentsInChildren<LODGroup>();
			UnityEngine.Debug.Log("Children " + componentsInChildren.Length + " VertexCount " + num + " TriangleCount " + num2 + " LODGroups " + componentsInChildren3.Length);
		}

		private void Update()
		{
			if (setLodGroups)
			{
				setLodGroups = false;
				SetLodGroupsActive(lodGroupsActive);
			}
		}

		public void SetLodGroupsActive(bool active)
		{
			LODGroup[] componentsInChildren = base.transform.GetComponentsInChildren<LODGroup>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = active;
			}
		}
	}
}
