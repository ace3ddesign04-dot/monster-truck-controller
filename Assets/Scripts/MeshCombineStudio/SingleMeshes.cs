using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	[Serializable]
	public class SingleMeshes
	{
		public Material mat;

		public List<MeshInfo> meshes = new List<MeshInfo>();

		public SingleMeshes(Transform t, Material mat, Mesh mesh)
		{
			this.mat = mat;
			meshes.Add(new MeshInfo(t, mesh));
		}
	}
}
