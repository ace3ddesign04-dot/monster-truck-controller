using UnityEngine;

namespace Battlehub.MeshTools
{
	public class MeshTransform
	{
		public Mesh Mesh;

		public Matrix4x4 Transform;

		public MeshTransform(Mesh mesh, Matrix4x4 transform)
		{
			Mesh = mesh;
			Transform = transform;
		}
	}
}
