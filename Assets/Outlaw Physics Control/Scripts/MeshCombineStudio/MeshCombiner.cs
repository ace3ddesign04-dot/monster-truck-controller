using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	public class MeshCombiner : MonoBehaviour
	{
		public enum HandleObjects
		{
			None,
			DisableRenderes,
			DisableGameObject,
			DisableParentGameObject,
			DeleteRenderers,
			DeleteGameObject,
			DeleteParentGameObject
		}

		public enum HandleLODGroups
		{
			None,
			Disable,
			Delete
		}

		[Serializable]
		public class SearchOptions
		{
			public GameObject parent;

			public bool drawGizmos = true;

			public bool searchBoxGridX = true;

			public bool searchBoxGridY = true;

			public bool searchBoxGridZ = true;

			public bool searchBoxSquare;

			public bool useVertexInputLimit;

			public int vertexInputLimit = 8000;

			public LayerMask layerMask = -1;

			public bool useTag;

			public string tag;

			public bool nameContains;

			public List<string> nameContainList = new List<string>();

			public bool onlyStatic = true;

			public SearchOptions(GameObject parent)
			{
				this.parent = parent;
			}
		}

		[Serializable]
		public class CachedGameObject
		{
			public GameObject go;

			public Transform t;

			public MeshRenderer mr;

			public MeshFilter mf;

			public CachedGameObject(GameObject go, Transform t, MeshRenderer mr, MeshFilter mf)
			{
				this.go = go;
				this.t = t;
				this.mr = mr;
				this.mf = mf;
			}
		}

		public static MeshCombiner instance;

		public List<Transform> combinedList = new List<Transform>();

		public ObjectOctree.Cell octree;

		[NonSerialized]
		public bool octreeCreated;

		public int cellSize = 32;

		public bool useVertexOutputLimit;

		public int vertexOutputLimit = 65534;

		public int[] lodObjectCount;

		public int[] lodObjectSearchCount;

		private int _vertexOutputLimit;

		public bool combineInRuntime;

		public bool combineOnStart = true;

		public bool useCombineSwapKey;

		public KeyCode combineSwapKey = KeyCode.Tab;

		public HandleObjects originalObjects = HandleObjects.DisableRenderes;

		public HandleLODGroups originalObjectsLODGroups = HandleLODGroups.Disable;

		public bool addMeshColliders;

		public int lodAmount = 1;

		public string lodSearchText = "LOD";

		public SearchOptions searchOptions;

		public Vector3 oldPosition;

		public Vector3 oldScale;

		private List<CachedGameObject> originalObjectList = new List<CachedGameObject>();

		private List<CachedGameObject> combinedMeshList = new List<CachedGameObject>();

		public bool combined;

		public CombinedLODManager combinedLODManager;

		public bool combinedActive;

		private List<Vector3> newVertices;

		private List<Vector3> newNormals;

		private List<Vector4> newTangents;

		private List<Color32> newColors;

		private List<int> newTriangles;

		private List<Vector2> newUvs1;

		private List<Vector2> newUvs2;

		private List<Vector2> newUvs3;

		private List<Vector2> newUvs4;

		private List<Vector3> vertices;

		private List<Vector3> normals;

		private List<Vector4> tangents;

		private List<Color32> colors;

		private List<Vector2> uvs1;

		private List<Vector2> uvs2;

		private List<Vector2> uvs3;

		private List<Vector2> uvs4;

		private List<int> triangles;

		private bool hasUv2;

		private bool hasUv3;

		private bool hasUv4;

		private bool hasColors;

		private int[] matTriangles;

		private int vertexCount;

		private int triangleCount;

		private int splitIndex;

		private int startIndex;

		private int totalVertexCount;

		private int totalTriangleCount;

		private int totalVertices;

		private int totalTriangles;

		private int totalCombined;

		private GameObject combinedGO;

		private GameObject uncombinedGO;

		private Bounds bounds;

		private int subTriangleCountOld;

		private int[] vertexTable = new int[65534];

		private void Awake()
		{
			instance = this;
			StartRuntime();
		}

		private void StartRuntime()
		{
			if (combineInRuntime && combineOnStart)
			{
				CombineLods();
				ExecuteHandleObjects(active: false);
			}
			if (useCombineSwapKey)
			{
				base.gameObject.AddComponent<SwapCombineKey>();
			}
		}

		private void OnDestroy()
		{
			instance = null;
		}

		private void GetBounds()
		{
			Vector3 position = base.transform.position;
			Vector3 lossyScale = base.transform.lossyScale;
			bounds = new Bounds(position + new Vector3(0f, lossyScale.y * 0.5f, 0f), base.transform.lossyScale);
		}

		private void InitLists()
		{
			newVertices = new List<Vector3>(65534);
			newNormals = new List<Vector3>(65534);
			newTangents = new List<Vector4>(65534);
			newColors = new List<Color32>(65534);
			newTriangles = new List<int>(196602);
			newUvs1 = new List<Vector2>(65534);
			newUvs2 = new List<Vector2>(65534);
			newUvs3 = new List<Vector2>(65534);
			newUvs4 = new List<Vector2>(65534);
			vertices = new List<Vector3>(65534);
			normals = new List<Vector3>(65534);
			tangents = new List<Vector4>(65534);
			colors = new List<Color32>(65534);
			uvs1 = new List<Vector2>(65534);
			uvs2 = new List<Vector2>(65534);
			uvs3 = new List<Vector2>(65534);
			uvs4 = new List<Vector2>(65534);
			triangles = new List<int>(196602);
		}

		private void GarbageCollectLists()
		{
			newVertices = (newNormals = null);
			newTangents = null;
			newUvs1 = (newUvs2 = (newUvs3 = (newUvs4 = null)));
			newColors = null;
			newTriangles = null;
			vertices = (normals = null);
			tangents = null;
			uvs1 = (uvs2 = (uvs3 = (uvs4 = null)));
			colors = null;
			triangles = null;
		}

		public void CalcOctreeSize()
		{
			Vector3 lossyScale = base.transform.lossyScale;
			float x = lossyScale.x;
			float num = x;
			int num2 = 0;
			while (num > (float)cellSize)
			{
				num /= 2f;
				num2++;
			}
			octree.maxLevels = num2;
			float num3 = (int)Mathf.Pow(2f, num2) * cellSize;
			ref Bounds reference = ref octree.bounds;
			Vector3 position = base.transform.position;
			Vector3 lossyScale2 = base.transform.lossyScale;
			reference.center = position + new Vector3(0f, lossyScale2.y * 0.5f, 0f);
			octree.bounds.size = new Vector3(num3, num3, num3);
		}

		public void ResetOctree()
		{
			octreeCreated = false;
			if (octree == null)
			{
				octree = new ObjectOctree.Cell();
				return;
			}
			totalCombined = 0;
			BaseOctree.Cell[] cells = octree.cells;
			octree.Reset(ref cells);
		}

		public void AddToOctree()
		{
			originalObjectList.Clear();
			combinedMeshList.Clear();
			ResetOctree();
			CalcOctreeSize();
			GetBounds();
			ObjectOctree.lodCount = lodAmount;
			ObjectOctree.MaxCell.maxCellCount = 0;
			lodObjectCount = new int[lodAmount];
			lodObjectSearchCount = new int[lodAmount];
			for (int i = 0; i < lodAmount; i++)
			{
				AddObjects(i);
			}
		}

		public void AddCombinedLODManager()
		{
			combinedLODManager = GetComponent<CombinedLODManager>();
			if (combinedLODManager == null)
			{
				combinedLODManager = base.gameObject.AddComponent<CombinedLODManager>();
			}
			combinedLODManager.UpdateLods(this, lodAmount);
		}

		public void DestroyCombinedLODManager()
		{
			combinedLODManager = GetComponent<CombinedLODManager>();
			if (combinedLODManager != null)
			{
				UnityEngine.Object.DestroyImmediate(combinedLODManager);
			}
		}

		public void DestroyCombinedGameObjects()
		{
			combined = false;
			for (int i = 0; i < combinedList.Count; i++)
			{
				if (combinedList[i] != null)
				{
					UnityEngine.Object.DestroyImmediate(combinedList[i].gameObject);
				}
			}
			combinedList.Clear();
		}

		public void SetCombinedGameObjects(bool active)
		{
			if (combinedLODManager != null)
			{
				combinedLODManager.enabled = active;
			}
			for (int i = 0; i < combinedList.Count; i++)
			{
				if (combinedList[i] != null)
				{
					combinedList[i].gameObject.SetActive(active);
				}
			}
		}

		public void SwapCombine()
		{
			if (!combined)
			{
				CombineLods();
			}
			combinedActive = !combinedActive;
			SetCombinedGameObjects(combinedActive);
			ExecuteHandleObjects(!combinedActive);
		}

		public void ExecuteHandleObjects(bool active)
		{
			if (originalObjects == HandleObjects.DisableRenderes)
			{
				for (int i = 0; i < originalObjectList.Count; i++)
				{
					originalObjectList[i].mr.enabled = active;
				}
			}
			else if (originalObjects == HandleObjects.DisableGameObject)
			{
				for (int j = 0; j < originalObjectList.Count; j++)
				{
					originalObjectList[j].go.SetActive(active);
				}
			}
			else if (originalObjects == HandleObjects.DisableParentGameObject)
			{
				for (int k = 0; k < originalObjectList.Count; k++)
				{
					CachedGameObject cachedGameObject = originalObjectList[k];
					if (cachedGameObject.t.parent != null)
					{
						cachedGameObject.t.parent.gameObject.SetActive(active);
					}
				}
			}
			else if (originalObjects == HandleObjects.DeleteRenderers)
			{
				for (int l = 0; l < originalObjectList.Count; l++)
				{
					CachedGameObject cachedGameObject2 = originalObjectList[l];
					UnityEngine.Object.Destroy(cachedGameObject2.mf);
					UnityEngine.Object.Destroy(cachedGameObject2.mr);
				}
			}
			else if (originalObjects == HandleObjects.DeleteGameObject)
			{
				for (int m = 0; m < originalObjectList.Count; m++)
				{
					CachedGameObject cachedGameObject3 = originalObjectList[m];
					if (cachedGameObject3.go != null)
					{
						UnityEngine.Object.Destroy(cachedGameObject3.go);
					}
				}
			}
			else if (originalObjects == HandleObjects.DeleteParentGameObject)
			{
				for (int n = 0; n < originalObjectList.Count; n++)
				{
					CachedGameObject cachedGameObject4 = originalObjectList[n];
					if (cachedGameObject4.t != null && cachedGameObject4.t.parent != null)
					{
						UnityEngine.Object.Destroy(cachedGameObject4.t.parent.gameObject);
					}
				}
			}
			if (originalObjectsLODGroups == HandleLODGroups.Disable)
			{
				for (int num = 0; num < originalObjectList.Count; num++)
				{
					CachedGameObject cachedGameObject5 = originalObjectList[num];
					if (cachedGameObject5.t != null)
					{
						LODGroup componentInParent = cachedGameObject5.t.GetComponentInParent<LODGroup>();
						if (componentInParent != null)
						{
							componentInParent.enabled = active;
						}
					}
				}
			}
			else
			{
				if (originalObjectsLODGroups != HandleLODGroups.Delete)
				{
					return;
				}
				for (int num2 = 0; num2 < originalObjectList.Count; num2++)
				{
					CachedGameObject cachedGameObject6 = originalObjectList[num2];
					if (cachedGameObject6.t != null)
					{
						LODGroup componentInParent2 = cachedGameObject6.t.GetComponentInParent<LODGroup>();
						if (componentInParent2 != null)
						{
							UnityEngine.Object.Destroy(componentInParent2);
						}
					}
				}
			}
		}

		public void CombineLods()
		{
			DestroyCombinedGameObjects();
			if (!octreeCreated || combined)
			{
				AddToOctree();
			}
			if (!octreeCreated)
			{
				return;
			}
			if (newVertices == null)
			{
				InitLists();
			}
			for (int i = 0; i < lodAmount; i++)
			{
				Combine(i);
			}
			if (lodAmount > 1)
			{
				if (combinedLODManager == null)
				{
					combinedLODManager = base.gameObject.AddComponent<CombinedLODManager>();
				}
				combinedLODManager.lods = new CombinedLODManager.LOD[lodAmount];
				combinedLODManager.distances = new float[lodAmount];
				for (int j = 0; j < lodAmount; j++)
				{
					combinedLODManager.lods[j] = new CombinedLODManager.LOD(combinedList[j]);
					combinedLODManager.distances[j] = j * cellSize;
				}
				combinedLODManager.ResetOctree();
				combinedLODManager.octreeCenter = octree.bounds.center;
				combinedLODManager.octreeSize = octree.bounds.size;
				combinedLODManager.maxLevels = octree.maxLevels;
			}
			combinedActive = true;
			combined = true;
			GarbageCollectLists();
		}

		public void Combine(int lodLevel)
		{
			uncombinedGO = new GameObject("_Umcombined");
			octree.UncombineMeshes(this, lodLevel);
			octree.SortObjects(lodLevel);
			combinedGO = new GameObject("Combined" + ((lodAmount <= 1) ? string.Empty : (" " + lodSearchText + lodLevel.ToString())));
			combinedGO.transform.parent = base.transform;
			if (useVertexOutputLimit)
			{
				_vertexOutputLimit = vertexOutputLimit;
			}
			else
			{
				_vertexOutputLimit = 65534;
			}
			octree.CombineMeshes(this, lodLevel);
			UnityEngine.Object.DestroyImmediate(uncombinedGO);
			combinedList.Add(combinedGO.transform);
		}

		public void AddObjects(int lodLevel)
		{
			if (searchOptions.parent == null)
			{
				UnityEngine.Debug.Log("You need to assign a 'Parent' GameObject in which meshes will be searched");
				return;
			}
			Transform[] componentsInChildren = searchOptions.parent.GetComponentsInChildren<Transform>();
			AddTransforms(componentsInChildren, lodLevel);
		}

		private void AddTransforms(Transform[] transforms, int lodLevel)
		{
			string value = lodSearchText + lodLevel.ToString();
			lodObjectSearchCount[lodLevel] = transforms.Length;
			foreach (Transform transform in transforms)
			{
				int num = 1 << transform.gameObject.layer;
				if ((searchOptions.layerMask.value & num) != num || (searchOptions.useTag && !transform.CompareTag(searchOptions.tag)))
				{
					continue;
				}
				MeshRenderer component = transform.GetComponent<MeshRenderer>();
				if (component == null || !bounds.Contains(component.bounds.center) || (searchOptions.onlyStatic && !transform.gameObject.isStatic))
				{
					continue;
				}
				MeshFilter component2 = transform.GetComponent<MeshFilter>();
				if (component2 == null)
				{
					continue;
				}
				Mesh sharedMesh = component2.sharedMesh;
				if (sharedMesh == null || (searchOptions.useVertexInputLimit && sharedMesh.vertexCount > searchOptions.vertexInputLimit))
				{
					continue;
				}
				if (searchOptions.nameContains)
				{
					bool flag = false;
					for (int j = 0; j < searchOptions.nameContainList.Count; j++)
					{
						if (Methods.Contains(transform.name, searchOptions.nameContainList[j]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
				}
				if (lodAmount <= 1 || transform.name.Contains(value))
				{
					lodObjectCount[lodLevel]++;
					int subMeshCount = sharedMesh.subMeshCount;
					if (octree.AddObject(transform, component, (subMeshCount <= 1) ? true : false, lodLevel))
					{
						originalObjectList.Add(new CachedGameObject(transform.gameObject, transform, component, component2));
					}
				}
			}
			if (lodObjectCount[lodLevel] > 0)
			{
				octreeCreated = true;
			}
			else
			{
				UnityEngine.Debug.Log("No matching GameObjects with LOD " + lodLevel + " 'Search Options' are found for combining.");
			}
		}

		public void CombineMeshes(SingleMeshes sortedMesh, Vector3 center)
		{
			totalCombined += sortedMesh.meshes.Count;
			splitIndex = 0;
			totalVertexCount = 0;
			totalTriangleCount = 0;
			int num = 0;
			bool flag;
			do
			{
				flag = CountVertices(sortedMesh);
				CombineMesh(sortedMesh, center);
				CreateMesh(null, combinedGO.transform, sortedMesh, center, 0, rotate: false, num++);
			}
			while (flag);
			splitIndex = 0;
		}

		private bool CountVertices(SingleMeshes sortedMesh)
		{
			totalVertices = 0;
			totalTriangles = 0;
			startIndex = splitIndex;
			bool flag = false;
			for (int i = splitIndex; i < sortedMesh.meshes.Count; i++)
			{
				MeshInfo meshInfo = sortedMesh.meshes[i];
				Mesh mesh = meshInfo.mesh;
				if (totalVertices + mesh.vertexCount > _vertexOutputLimit)
				{
					splitIndex = i;
					flag = true;
					break;
				}
				totalVertices += mesh.vertexCount;
				totalTriangles += (int)mesh.GetIndexCount(0);
			}
			if (!flag)
			{
				splitIndex = sortedMesh.meshes.Count;
			}
			return flag;
		}

		private void ClearNewLists()
		{
			newVertices.Clear();
			newNormals.Clear();
			newTangents.Clear();
			newUvs1.Clear();
			newUvs2.Clear();
			newUvs3.Clear();
			newUvs4.Clear();
			newColors.Clear();
			newTriangles.Clear();
		}

		private void CombineMesh(SingleMeshes sortedMesh, Vector3 center)
		{
			totalVertexCount = 0;
			totalTriangleCount = 0;
			ClearNewLists();
			for (int i = startIndex; i < splitIndex; i++)
			{
				MeshInfo meshInfo = sortedMesh.meshes[i];
				Transform t = meshInfo.t;
				Vector3 position = t.position;
				Quaternion rotation = t.rotation;
				Vector3 lossyScale = t.lossyScale;
				Vector3 b = position - center;
				Mesh mesh = meshInfo.mesh;
				mesh.GetVertices(vertices);
				mesh.GetTriangles(triangles, 0);
				mesh.GetNormals(normals);
				mesh.GetTangents(tangents);
				hasUv2 = (hasUv3 = (hasUv4 = (hasColors = false)));
				vertexCount = vertices.Count;
				triangleCount = triangles.Count;
				mesh.GetUVs(0, uvs1);
				mesh.GetUVs(1, uvs2);
				mesh.GetUVs(2, uvs3);
				mesh.GetUVs(3, uvs4);
				mesh.GetColors(colors);
				if (uvs2.Count > 0)
				{
					hasUv2 = true;
				}
				if (uvs3.Count > 0)
				{
					hasUv3 = true;
				}
				if (uvs4.Count > 0)
				{
					hasUv4 = true;
				}
				if (colors.Count > 0)
				{
					hasColors = true;
				}
				for (int j = 0; j < vertexCount; j++)
				{
					Vector3 a = t.TransformPoint(vertices[j]) - position;
					newVertices.Add(a + b);
					newNormals.Add(rotation * normals[j]);
					Vector4 item = rotation * tangents[j];
					Vector4 vector = tangents[j];
					item.w = vector.w;
					newTangents.Add(item);
					newUvs1.Add(uvs1[j]);
					if (hasUv2)
					{
						newUvs2.Add(uvs2[j]);
					}
					if (hasUv3)
					{
						newUvs3.Add(uvs3[j]);
					}
					if (hasUv4)
					{
						newUvs4.Add(uvs4[j]);
					}
					if (hasColors)
					{
						newColors.Add(colors[j]);
					}
				}
				for (int k = 0; k < triangleCount; k++)
				{
					newTriangles.Add(triangles[k] + totalVertexCount);
				}
				totalVertexCount += vertexCount;
				totalTriangleCount += triangleCount;
			}
		}

		private string ClusterName(string name)
		{
			int length = name.Length;
			for (int num = length - 1; num >= 0; num -= 2)
			{
				name = name.Insert(num, "-");
			}
			return name;
		}

		private MeshRenderer CreateMesh(Transform t, Transform parent, SingleMeshes sortedMesh, Vector3 center, int matIndex, bool rotate, int meshIndex)
		{
			string text = (!(t != null)) ? sortedMesh.mat.name : t.name;
			GameObject gameObject = new GameObject(text);
			Transform transform = gameObject.transform;
			transform.parent = parent;
			transform.position = center;
			if (rotate)
			{
				transform.rotation = t.rotation;
			}
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			Mesh mesh = new Mesh();
			mesh.name = text + "_" + meshIndex;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			if (t != null)
			{
				meshRenderer.sharedMaterial = t.GetComponent<MeshRenderer>().sharedMaterials[matIndex];
			}
			else
			{
				meshRenderer.sharedMaterial = sortedMesh.mat;
			}
			mesh.SetVertices(newVertices);
			mesh.SetTriangles(newTriangles, 0);
			mesh.SetTangents(newTangents);
			mesh.SetNormals(newNormals);
			mesh.SetUVs(0, newUvs1);
			if (hasUv2)
			{
				mesh.SetUVs(1, newUvs2);
			}
			if (hasUv3)
			{
				mesh.SetUVs(2, newUvs3);
			}
			if (hasUv4)
			{
				mesh.SetUVs(3, newUvs4);
			}
			if (hasColors)
			{
				mesh.SetColors(newColors);
			}
			meshFilter.sharedMesh = mesh;
			if (addMeshColliders)
			{
				gameObject.AddComponent<MeshCollider>();
			}
			gameObject.AddComponent<GarbageCollectMesh>();
			if (rotate)
			{
				combinedMeshList.Add(new CachedGameObject(gameObject, transform, meshRenderer, meshFilter));
			}
			return meshRenderer;
		}

		public void DisplayUVs(List<Vector2> uv)
		{
			for (int i = 0; i < uv.Count; i++)
			{
				Vector2 vector = uv[i];
				object arg = vector.x;
				Vector2 vector2 = uv[i];
				UnityEngine.Debug.Log(arg + " " + vector2.y);
			}
		}

		public void DisplayColors(Mesh mesh)
		{
			Color32[] colors = mesh.colors32;
			for (int i = 0; i < colors.Length; i++)
			{
				UnityEngine.Debug.Log(colors[i].r + " " + colors[i].g + " " + colors[i].b + " " + colors[i].a);
			}
		}

		public void UncombineMeshes(List<Transform> transforms, int lodLevel)
		{
			for (int i = 0; i < transforms.Count; i++)
			{
				Transform t = transforms[i];
				UncombineMesh(t, lodLevel);
			}
		}

		public int UncombineMesh(Transform t, int lodLevel)
		{
			Mesh sharedMesh = t.GetComponent<MeshFilter>().sharedMesh;
			int num = t.GetComponent<MeshRenderer>().sharedMaterials.Length;
			int subMeshCount = sharedMesh.subMeshCount;
			int num2 = (subMeshCount <= num) ? subMeshCount : num;
			Vector3 lossyScale = t.lossyScale;
			if (num2 > 1)
			{
				vertices.Clear();
				normals.Clear();
				tangents.Clear();
				uvs1.Clear();
				uvs2.Clear();
				uvs3.Clear();
				uvs4.Clear();
				colors.Clear();
				sharedMesh.GetVertices(vertices);
				sharedMesh.GetNormals(normals);
				sharedMesh.GetTangents(tangents);
				hasUv2 = (hasUv3 = (hasUv4 = (hasColors = false)));
				sharedMesh.GetUVs(0, uvs1);
				sharedMesh.GetUVs(1, uvs2);
				sharedMesh.GetUVs(2, uvs3);
				sharedMesh.GetUVs(3, uvs4);
				sharedMesh.GetColors(colors);
				if (uvs2.Count > 0)
				{
					hasUv2 = true;
				}
				if (uvs3.Count > 0)
				{
					hasUv3 = true;
				}
				if (uvs4.Count > 0)
				{
					hasUv4 = true;
				}
				if (colors.Count > 0)
				{
					hasColors = true;
				}
				for (int i = 0; i < num2; i++)
				{
					ClearNewLists();
					int num3 = 0;
					triangles.Clear();
					sharedMesh.GetTriangles(triangles, i);
					for (int j = 0; j < triangles.Count; j++)
					{
						vertexTable[triangles[j]] = -1;
					}
					for (int k = 0; k < triangles.Count; k++)
					{
						int num4 = triangles[k];
						if (vertexTable[num4] == -1)
						{
							newVertices.Add(Vector3.Scale(vertices[num4], lossyScale));
							newNormals.Add(normals[num4]);
							newTangents.Add(tangents[num4]);
							newUvs1.Add(uvs1[num4]);
							if (hasUv2)
							{
								newUvs2.Add(uvs2[num4]);
							}
							if (hasUv3)
							{
								newUvs3.Add(uvs3[num4]);
							}
							if (hasUv4)
							{
								newUvs4.Add(uvs4[num4]);
							}
							if (hasColors)
							{
								newColors.Add(colors[num4]);
							}
							newTriangles.Add(num3);
							vertexTable[num4] = num3++;
						}
						else
						{
							newTriangles.Add(vertexTable[num4]);
						}
					}
					MeshRenderer meshRenderer = CreateMesh(t, uncombinedGO.transform, null, t.position, i, rotate: true, i);
					octree.AddObject(meshRenderer.transform, meshRenderer, addToSingle: true, lodLevel);
				}
			}
			return num2;
		}

		private void OnDrawGizmosSelected()
		{
			if (!searchOptions.drawGizmos)
			{
				return;
			}
			Vector3 lossyScale = base.transform.lossyScale;
			int num = Mathf.CeilToInt(Mathf.Ceil(lossyScale.x / (float)cellSize) / 2f) * 2;
			int num2 = Mathf.CeilToInt(lossyScale.y / (float)cellSize);
			int num3 = Mathf.CeilToInt(Mathf.Ceil(lossyScale.z / (float)cellSize) / 2f) * 2;
			lossyScale = new Vector3(num * cellSize, num2 * cellSize, num3 * cellSize);
			Vector3 a = base.transform.position - new Vector3(lossyScale.x * 0.5f, 0f, lossyScale.z * 0.5f);
			Vector3 b = new Vector3((float)cellSize * 0.5f, 0f, (float)cellSize * 0.5f);
			Gizmos.color = Color.white;
			if (octree != null && octreeCreated)
			{
				octree.Draw(onlyMaxLevel: true);
			}
			else
			{
				if (searchOptions.searchBoxGridX)
				{
					for (int i = 0; i < num; i++)
					{
						for (int j = 0; j < num3; j++)
						{
							Gizmos.DrawWireCube(a + b + new Vector3(i * cellSize, 0 * cellSize, j * cellSize), new Vector3(cellSize, 0f, cellSize));
						}
					}
				}
				if (searchOptions.searchBoxGridZ)
				{
					for (int k = 0; k < num; k++)
					{
						for (int l = 0; l < num2; l++)
						{
							Gizmos.DrawWireCube(a + new Vector3((float)cellSize * 0.5f, (float)cellSize * 0.5f, 0f) + new Vector3(k * cellSize, l * cellSize, num3 * cellSize), new Vector3(cellSize, cellSize, 0f));
						}
					}
				}
				if (searchOptions.searchBoxGridY)
				{
					for (int m = 0; m < num3; m++)
					{
						for (int n = 0; n < num2; n++)
						{
							Gizmos.DrawWireCube(a + new Vector3(0f, (float)cellSize * 0.5f, (float)cellSize * 0.5f) + new Vector3(0 * cellSize, n * cellSize, m * cellSize), new Vector3(0f, cellSize, cellSize));
						}
					}
				}
				Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
				if (searchOptions.searchBoxGridX)
				{
					Gizmos.DrawCube(base.transform.position, new Vector3(lossyScale.x, 0f, lossyScale.z));
				}
				if (searchOptions.searchBoxGridY)
				{
					Gizmos.DrawCube(base.transform.position + new Vector3((float)(-(num * cellSize)) * 0.5f, (float)(num2 * cellSize) * 0.5f, 0f), new Vector3(0f, lossyScale.y, lossyScale.z));
				}
				if (searchOptions.searchBoxGridZ)
				{
					Gizmos.DrawCube(base.transform.position + new Vector3(0f, (float)(num2 * cellSize) * 0.5f, (float)(num3 * cellSize) * 0.5f), new Vector3(lossyScale.x, lossyScale.y, 0f));
				}
				Gizmos.color = Color.white;
			}
			GetBounds();
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
			Gizmos.color = Color.white;
		}
	}
}
