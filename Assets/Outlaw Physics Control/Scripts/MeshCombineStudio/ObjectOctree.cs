using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	public class ObjectOctree
	{
		public class LOD
		{
			public List<Transform> transforms = new List<Transform>();

			public List<Transform> singleTransforms = new List<Transform>();

			public List<SingleMeshes> sortedMeshes;

			public int vertCount;

			public int objectCount;

			public int GetSortMeshIndex(Material mat)
			{
				for (int i = 0; i < sortedMeshes.Count; i++)
				{
					if (mat == null)
					{
						UnityEngine.Debug.Log("Material null");
					}
					if (sortedMeshes[i].mat == null)
					{
						UnityEngine.Debug.Log("Sorted mat null");
					}
					if (sortedMeshes[i].mat.name == mat.name && sortedMeshes[i].mat.shader == mat.shader && (!mat.HasProperty("_MainTex") || (mat.HasProperty("_MainTex") && sortedMeshes[i].mat.GetTexture("_MainTex") == mat.GetTexture("_MainTex"))))
					{
						return i;
					}
				}
				return -1;
			}
		}

		public class MaxCell : Cell
		{
			public static int maxCellCount;

			public LOD[] lods;
		}

		public class Cell : BaseOctree.Cell
		{
			public new Cell[] cells;

			public Cell()
			{
			}

			public Cell(Vector3 position, Vector3 size, int maxLevels)
				: base(position, size, maxLevels)
			{
			}

			public bool AddObject(Transform t, MeshRenderer mr, bool addToSingle, int lodLevel)
			{
				Vector3 position = t.position;
				if (InsideBounds(position))
				{
					AddObjectInternal(t, position, addToSingle, lodLevel);
					return true;
				}
				return false;
			}

			private void AddObjectInternal(Transform t, Vector3 position, bool addToSingle, int lodLevel)
			{
				if (level == maxLevels)
				{
					MaxCell maxCell = (MaxCell)this;
					if (maxCell.lods == null)
					{
						maxCell.lods = new LOD[lodCount];
					}
					if (maxCell.lods[lodLevel] == null)
					{
						maxCell.lods[lodLevel] = new LOD();
					}
					LOD lOD = maxCell.lods[lodLevel];
					if (lOD.transforms == null)
					{
						lOD.transforms = new List<Transform>();
					}
					if (addToSingle)
					{
						lOD.singleTransforms.Add(t);
					}
					else
					{
						lOD.transforms.Add(t);
					}
					lOD.objectCount++;
					MeshFilter component = t.GetComponent<MeshFilter>();
					Mesh sharedMesh = component.sharedMesh;
					lOD.vertCount += sharedMesh.vertexCount;
				}
				else
				{
					bool maxCellCreated;
					int num = AddCell<Cell, MaxCell>(ref cells, position, out maxCellCreated);
					if (maxCellCreated)
					{
						MaxCell.maxCellCount++;
					}
					cells[num].AddObjectInternal(t, position, addToSingle, lodLevel);
				}
			}

			public void SortObjects(int lodLevel)
			{
				if (level == maxLevels)
				{
					MaxCell maxCell = (MaxCell)this;
					LOD lOD = maxCell.lods[lodLevel];
					if (lOD == null)
					{
						return;
					}
					lOD.sortedMeshes = new List<SingleMeshes>();
					for (int i = 0; i < lOD.singleTransforms.Count; i++)
					{
						Transform transform = lOD.singleTransforms[i];
						MeshFilter component = transform.GetComponent<MeshFilter>();
						MeshRenderer component2 = transform.GetComponent<MeshRenderer>();
						Material sharedMaterial = component2.sharedMaterial;
						Mesh sharedMesh = component.sharedMesh;
						int sortMeshIndex = lOD.GetSortMeshIndex(sharedMaterial);
						if (sortMeshIndex != -1)
						{
							lOD.sortedMeshes[sortMeshIndex].meshes.Add(new MeshInfo(transform, sharedMesh));
						}
						else
						{
							lOD.sortedMeshes.Add(new SingleMeshes(transform, sharedMaterial, sharedMesh));
						}
					}
					return;
				}
				for (int j = 0; j < 8; j++)
				{
					if (cellsUsed[j])
					{
						cells[j].SortObjects(lodLevel);
					}
				}
			}

			public void SetObjectsActive(bool active, int lodLevel)
			{
				if (level == maxLevels)
				{
					MaxCell maxCell = (MaxCell)this;
					LOD lOD = maxCell.lods[lodLevel];
					for (int i = 0; i < lOD.sortedMeshes.Count; i++)
					{
					}
					return;
				}
				for (int j = 0; j < 8; j++)
				{
					if (cellsUsed[j])
					{
						cells[j].SetObjectsActive(active, lodLevel);
					}
				}
			}

			public void CombineMeshes(MeshCombiner meshCombiner, int lodLevel)
			{
				if (level == maxLevels)
				{
					MaxCell maxCell = (MaxCell)this;
					LOD lOD = maxCell.lods[lodLevel];
					if (lOD != null)
					{
						for (int i = 0; i < lOD.sortedMeshes.Count; i++)
						{
							meshCombiner.CombineMeshes(lOD.sortedMeshes[i], bounds.center);
						}
					}
					return;
				}
				for (int j = 0; j < 8; j++)
				{
					if (cellsUsed[j])
					{
						cells[j].CombineMeshes(meshCombiner, lodLevel);
					}
				}
			}

			public void UncombineMeshes(MeshCombiner meshCombiner, int lodLevel)
			{
				if (level == maxLevels)
				{
					MaxCell maxCell = (MaxCell)this;
					LOD lOD = maxCell.lods[lodLevel];
					if (lOD == null)
					{
						UnityEngine.Debug.Log("-------------");
						for (int i = 0; i < 3; i++)
						{
							LOD lOD2 = maxCell.lods[i];
							if (lOD2 == null)
							{
								UnityEngine.Debug.Log(i);
							}
						}
						UnityEngine.Debug.Log("-------------");
					}
					else
					{
						meshCombiner.UncombineMeshes(lOD.transforms, lodLevel);
					}
					return;
				}
				for (int j = 0; j < 8; j++)
				{
					if (cellsUsed[j])
					{
						if (cells[j] == null)
						{
							UnityEngine.Debug.Log(j);
						}
						cells[j].UncombineMeshes(meshCombiner, lodLevel);
					}
				}
			}

			public void Draw(bool onlyMaxLevel)
			{
				if (!onlyMaxLevel || level == maxLevels)
				{
					Gizmos.DrawWireCube(bounds.center, bounds.size);
					if (level == maxLevels)
					{
						return;
					}
				}
				if (cells == null)
				{
					UnityEngine.Debug.Log(level);
				}
				if (cellsUsed == null)
				{
					UnityEngine.Debug.Log("f " + level);
				}
				for (int i = 0; i < 8; i++)
				{
					if (cellsUsed[i])
					{
						cells[i].Draw(onlyMaxLevel);
					}
				}
			}
		}

		public static int lodCount;
	}
}
