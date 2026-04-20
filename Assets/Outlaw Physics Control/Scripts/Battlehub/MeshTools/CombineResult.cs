using UnityEngine;

namespace Battlehub.MeshTools
{
	public class CombineResult
	{
		public GameObject GameObject;

		public Mesh Mesh;

		public CombineResult(GameObject gameObject, Mesh mesh)
		{
			GameObject = gameObject;
			Mesh = mesh;
		}
	}
}
