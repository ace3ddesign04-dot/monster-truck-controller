using System;
using UnityEngine;

namespace MeshCombineStudio
{
	[Serializable]
	public class MeshInfo
	{
		public Transform t;

		public Mesh mesh;

		public MeshInfo(Transform t, Mesh mesh)
		{
			this.t = t;
			this.mesh = mesh;
		}
	}
}
