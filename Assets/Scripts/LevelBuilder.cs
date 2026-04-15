using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
	private LevelEditorResources editorResources;

	private string mapName;

	private string mapDescription;

	private int mapRating;

	private bool mapUploaded;

	private bool mapVisible;

	private int seed;

	private int terrainSize;

	private float bumpsStrength;

	private bool flatCenter;

	private float treesDensity;

	private bool treesByEdgesOnly;

	private bool waterEnabled;

	private bool frozenWater;

	private float waterHeight;

	private int mainTextureID;

	private int lastPlacedExtraObject;

	private List<int> ignoredExtraObjectIDs = new List<int>();

	private ExtraObjectReference[] usedExtraObjects;

	private List<TerrainStamp> stamps;

	private List<MudStamp> mudStamps = new List<MudStamp>();

	private List<TerrainPath> paths;

	private List<string> propsStrings;

	private List<string> routeStrings;

	private List<TreeInstance> treesAddedForPreview = new List<TreeInstance>();

	private List<TreeInstance> treesRemovedForPreview = new List<TreeInstance>();

	private List<GameObject> extraObjectsAddedForPreview = new List<GameObject>();

	private List<GameObject> extraObjectsRemovedForPreview = new List<GameObject>();

	private float[,] defHeights;

	private Transform waterPlane;

	public bool playingScene;

	public bool editScene;

	private LevelWebHandler GetWebHandler
	{
		get
		{
			if (GetComponent<LevelWebHandler>() != null)
			{
				return GetComponent<LevelWebHandler>();
			}
			return base.gameObject.AddComponent<LevelWebHandler>();
		}
	}

	private Terrain terrain => Terrain.activeTerrain;

	private void Awake()
	{
		waterPlane = GameObject.Find("WaterPlane").transform;
		LevelEditorTools.ResetTerrain(terrain);
	}

	private void Start()
	{
		if (GameState.mapToDownload != null && GameState.mapToDownload != string.Empty)
		{
			DownloadLevel(GameState.mapToDownload);
			return;
		}
		GetComponent<LevelEditor>().seed = UnityEngine.Random.Range(0, 10000);
		GetComponent<LevelEditorUI>().InitializeUI();
		GetComponent<LevelEditor>().GenerateTerrain(dontShowNotification: true);
		GetComponent<LevelEditor>().ChangeLevelCreationStep(LevelCreationStep.Generation);
	}

	[ContextMenu("Upload level")]
	private void UploadLevel()
	{
		UploadLevel(GetComponent<LevelEditor>());
	}

	[ContextMenu("Upload 100 maps")]
	private void Upload100Maps()
	{
		StartCoroutine(Upload100MapsCor());
	}

	private IEnumerator Upload100MapsCor()
	{
		for (int i = 0; i < 3000; i++)
		{
			GetComponent<LevelEditor>().mapID = string.Empty;
			string chars = "ABCDEFGHIGKLMNOPQRSTUVWXYZ";
			string randMapName6 = string.Empty;
			randMapName6 += chars[UnityEngine.Random.Range(0, chars.Length)];
			randMapName6 += chars[UnityEngine.Random.Range(0, chars.Length)];
			randMapName6 += chars[UnityEngine.Random.Range(0, chars.Length)];
			randMapName6 += chars[UnityEngine.Random.Range(0, chars.Length)];
			randMapName6 += chars[UnityEngine.Random.Range(0, chars.Length)];
			GetComponent<LevelEditor>().mapName = randMapName6;
			GetComponent<LevelEditor>().mapDescription = "Description";
			GetComponent<LevelEditor>().mapVisible = true;
			UploadLevel(GetComponent<LevelEditor>());
			LevelEditorTools.AddMapToMyMaps(GetComponent<LevelEditor>().mapID);
			yield return null;
		}
	}

	public void UploadLevel(LevelEditor editor, Action successCallback = null, Action errorCallback = null)
	{
		editor.CheckRoutesLenghts();
		string text = editor.mapID;
		if (text == string.Empty || text == null)
		{
			text = (editor.mapID = LevelEditorTools.RandomName());
		}
		string empty = string.Empty;
		string text2 = editor.mapName;
		string text3 = editor.mapDescription;
		text2 = Utility.CleanBadWords(text2);
		text3 = Utility.CleanBadWords(text3);
		empty = empty + text2 + "\n";
		empty = empty + text3 + "\n";
		empty = empty + editor.mapRating + "\n";
		empty = empty + editor.mapVisible.ToString() + "\n";
		empty = empty + GameState.playerName + "\n";
		empty = empty + editor.seed.ToString() + "\n";
		empty = empty + ((int)editor.terrainSize).ToString() + "\n";
		empty = empty + editor.bumpsStrength.ToString() + "\n";
		empty = empty + editor.flatCenter.ToString() + "\n";
		empty = empty + editor.treesDensity.ToString() + "\n";
		empty = empty + editor.treesByEdgesOnly.ToString() + "\n";
		empty = empty + editor.waterEnabled.ToString() + "\n";
		empty = empty + editor.frozenWater.ToString() + "\n";
		empty = empty + editor.waterHeight.ToString() + "\n";
		empty = empty + editor.mainTextureID.ToString() + "\n";
		string str = editor.usedExtraObjects[0].arrayID + "|" + editor.usedExtraObjects[0].density + "|" + editor.usedExtraObjects[0].onlyByEdges.ToString();
		string str2 = editor.usedExtraObjects[1].arrayID + "|" + editor.usedExtraObjects[1].density + "|" + editor.usedExtraObjects[1].onlyByEdges.ToString();
		string str3 = editor.usedExtraObjects[2].arrayID + "|" + editor.usedExtraObjects[2].density + "|" + editor.usedExtraObjects[2].onlyByEdges.ToString();
		empty = empty + str + "\n";
		empty = empty + str2 + "\n";
		empty = empty + str3 + "\n";
		string text4 = string.Empty;
		foreach (int ignoredExtraObjectID in editor.ignoredExtraObjectIDs)
		{
			text4 = text4 + ignoredExtraObjectID + "|";
		}
		empty = empty + text4 + "\n";
		empty = empty + editor.mudStamps.Count + "\n";
		for (int i = 0; i < editor.mudStamps.Count; i++)
		{
			empty = empty + editor.mudStamps[i].Serialize() + "\n";
		}
		empty = empty + editor.stamps.Count.ToString() + "\n";
		for (int j = 0; j < editor.stamps.Count; j++)
		{
			string str4 = editor.stamps[j].Serialize();
			empty = empty + str4 + "\n";
		}
		empty = empty + editor.paths.Count.ToString() + "\n";
		for (int k = 0; k < editor.paths.Count; k++)
		{
			string str5 = editor.paths[k].Serialize();
			empty = empty + str5 + "\n";
		}
		Prop[] componentsInChildren = LevelEditorTools.PropsParent.GetComponentsInChildren<Prop>();
		empty = empty + componentsInChildren.Length.ToString() + "\n";
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			empty = empty + componentsInChildren[l].Serialize() + "\n";
		}
		empty = empty + editor.routes.Count.ToString() + "\n";
		for (int m = 0; m < editor.routes.Count; m++)
		{
			empty = empty + editor.routes[m].Serialize() + "\n";
		}
		LevelWebHandler getWebHandler = GetWebHandler;
		getWebHandler.UploadLevel(empty, text, successCallback, errorCallback);
	}

	public void DownloadLevel(string fileName)
	{
		if (editScene)
		{
			LevelEditor component = GetComponent<LevelEditor>();
			if (component.OnProcessingTerrainStarted != null)
			{
				component.OnProcessingTerrainStarted("Downloading map...");
			}
		}
		LevelWebHandler getWebHandler = GetWebHandler;
		getWebHandler.DownloadLevel(fileName, OnLevelDownloaded);
	}

	public void OnLevelDownloaded(string levelData)
	{
		UnityEngine.Debug.Log("level loaded");
		UnityEngine.Debug.Log(levelData);
		editorResources = LevelEditorTools.editorResources;
		string[] array = levelData.Split('\n');
		mapName = array[0];
		mapDescription = array[1];
		mapRating = int.Parse(array[2]);
		mapVisible = bool.Parse(array[3]);
		seed = int.Parse(array[5]);
		terrainSize = int.Parse(array[6]);
		bumpsStrength = float.Parse(array[7]);
		flatCenter = bool.Parse(array[8]);
		treesDensity = float.Parse(array[9]);
		treesByEdgesOnly = bool.Parse(array[10]);
		waterEnabled = bool.Parse(array[11]);
		frozenWater = bool.Parse(array[12]);
		waterHeight = float.Parse(array[13]);
		mainTextureID = int.Parse(array[14]);
		usedExtraObjects = new ExtraObjectReference[3];
		int num = 15;
		for (int i = 0; i < 3; i++)
		{
			string text = array[num];
			string[] array2 = text.Split('|');
			usedExtraObjects[i] = new ExtraObjectReference();
			usedExtraObjects[i].arrayID = int.Parse(array2[0]);
			usedExtraObjects[i].density = float.Parse(array2[1]);
			usedExtraObjects[i].onlyByEdges = bool.Parse(array2[2]);
			num++;
		}
		string text2 = array[num];
		ignoredExtraObjectIDs = new List<int>();
		string[] array3 = text2.Split('|');
		string[] array4 = array3;
		foreach (string text3 in array4)
		{
			if (text3 != string.Empty)
			{
				ignoredExtraObjectIDs.Add(int.Parse(text3));
			}
		}
		num++;
		int num2 = int.Parse(array[num]);
		num++;
		mudStamps = new List<MudStamp>();
		for (int k = 0; k < num2; k++)
		{
			string data = array[num];
			MudStamp mudStamp = new MudStamp();
			mudStamp.Deserialize(data);
			mudStamps.Add(mudStamp);
			num++;
		}
		stamps = new List<TerrainStamp>();
		int num3 = int.Parse(array[num]);
		num++;
		for (int l = 0; l < num3; l++)
		{
			string s = array[num];
			TerrainStamp terrainStamp = new TerrainStamp();
			terrainStamp.Deserialize(s);
			stamps.Add(terrainStamp);
			num++;
		}
		paths = new List<TerrainPath>();
		int num4 = int.Parse(array[num]);
		num++;
		for (int m = 0; m < num4; m++)
		{
			string data2 = array[num];
			TerrainPath terrainPath = new TerrainPath();
			terrainPath.Deserialize(data2);
			paths.Add(terrainPath);
			num++;
		}
		int num5 = int.Parse(array[num]);
		num++;
		propsStrings = new List<string>();
		for (int n = 0; n < num5; n++)
		{
			string item = array[num];
			propsStrings.Add(item);
			num++;
		}
		int num6 = int.Parse(array[num]);
		num++;
		routeStrings = new List<string>();
		for (int num7 = 0; num7 < num6; num7++)
		{
			string item2 = array[num];
			routeStrings.Add(item2);
			num++;
		}
		StartCoroutine(BuildMap());
	}

	private IEnumerator BuildMap()
	{
		LevelEditor editor = GetComponent<LevelEditor>();
		if (editScene && editor.OnProcessingTerrainStarted != null)
		{
			editor.OnProcessingTerrainStarted("Building terrain...");
		}
		LevelEditorTools.ResetTerrain(terrain);
		LevelEditorTools.GenerateStampBasedTerrain(terrain, editorResources.terrainGenerationStamps, seed, terrainSize, flatCenter, bumpsStrength);
		LevelEditorTools.ApplyWater(terrain, waterPlane, waterEnabled, terrainSize, waterHeight, frozenWater, editorResources.frozenWaterMaterial, editorResources.waterMaterial);
		LevelEditorTools.PlaceTrees(terrain, waterPlane, seed, terrainSize, editorResources.maxTreesCount, treesDensity, treesByEdgesOnly);
		lastPlacedExtraObject = LevelEditorTools.PlaceExtraObjects(terrain, waterPlane, seed, usedExtraObjects, editorResources.extraObjectsDictionary, terrainSize, lastPlacedExtraObject);
		LevelEditorTools.PlaceCliffs(terrain, editorResources.cliffPrefabs, terrainSize, seed, editorResources.baseCliffsCount, editorResources.minHillAngle);
		LevelEditorTools.PaintBaseTexture(terrain, mainTextureID);
		LevelEditorTools.PaintRockAndWaterTextures(terrain, waterPlane, waterEnabled, frozenWater, editorResources.underwaterTextureID, editorResources.rockTextureID, editorResources.minRockAngle, editorResources.maxRockAngle, mainTextureID);
		defHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
		yield return null;
		ApplyAllStamps();
		ApplyAllPaths();
		terrain.GetComponent<TerrainCollider>().enabled = false;
		terrain.GetComponent<TerrainCollider>().enabled = true;
		yield return null;
		PlaceProps();
		PlaceRoutes();
		LevelEditorTools.RemoveIgnoredExtraObjects(ignoredExtraObjectIDs);
		CorrectExtraObjectsTransforms();
		CorrectCliffsPositions();
		yield return null;
		VehicleLoader.Instance.SpawnPoints = GetSpawnPoints();
		if (playingScene)
		{
			VehicleLoader.Instance.loadVehicle = true;
			PlayerRouteRacingManager.Instance.Initialize();
			Transform[] spawnPoints = GetSpawnPoints();
			if (spawnPoints != null)
			{
				Transform[] array = spawnPoints;
				foreach (Transform transform in array)
				{
					transform.gameObject.SetActive(value: false);
				}
			}
			Prop[] componentsInChildren = LevelEditorTools.PropsParent.GetComponentsInChildren<Prop>();
			foreach (Prop prop in componentsInChildren)
			{
				prop.BakeProp();
			}
			SurfaceManager.Instance.FindWater();
			LevelEditorTools.ApplyMudStamps(mudStamps, terrain);
			SurfaceManager.Instance.CreateMudTerrains(mudStamps);
			SurfaceManager.Instance.FindWater();
		}
		if (editScene)
		{
			editor.lastPlacedExtraObjectID = lastPlacedExtraObject;
			editor.ignoredExtraObjectIDs = ignoredExtraObjectIDs;
			editor.mapName = mapName;
			editor.mapDescription = mapDescription;
			editor.mapRating = mapRating;
			editor.mapUploaded = true;
			editor.mapVisible = mapVisible;
			editor.seed = seed;
			editor.terrainSize = terrainSize;
			editor.bumpsStrength = bumpsStrength;
			editor.flatCenter = flatCenter;
			editor.treesDensity = treesDensity;
			editor.treesByEdgesOnly = treesByEdgesOnly;
			editor.waterEnabled = waterEnabled;
			editor.frozenWater = frozenWater;
			editor.waterHeight = waterHeight;
			editor.mainTextureID = mainTextureID;
			editor.usedExtraObjects = usedExtraObjects;
			editor.paths = paths;
			editor.stamps = stamps;
			editor.routes = new List<PlayerRoute>(LevelEditorTools.RoutesParent.GetComponentsInChildren<PlayerRoute>(includeInactive: true));
			editor.mudStamps = mudStamps;
			foreach (MudStamp mudStamp in mudStamps)
			{
				mudStamp.stampIndicator = LevelEditorTools.CreateMudIndicator(mudStamp, terrain);
			}
			editor.SelectMudStamp(-1);
			editor.mapID = GameState.mapToDownload;
			GetComponent<LevelEditorUI>().InitializeUI();
			if (editor.OnTerrainGenerated != null)
			{
				editor.OnTerrainGenerated();
			}
			if (editor.OnProcessingTerrainFinished != null)
			{
				editor.OnProcessingTerrainFinished();
			}
		}
	}

	private void ApplyAllStamps()
	{
		foreach (TerrainStamp stamp in stamps)
		{
			switch (stamp.stampAction)
			{
			case ModAction.LandscapeRaising:
				ApplyHeightStamp(stamp);
				PaintRockAndWaterTextures();
				CorrectExtraObjectsTransforms();
				break;
			case ModAction.LandscapeLowering:
				ApplyHeightStamp(stamp);
				PaintRockAndWaterTextures();
				CorrectExtraObjectsTransforms();
				break;
			case ModAction.Smoothing:
				ApplySmoothStamp(stamp);
				PaintRockAndWaterTextures();
				CorrectExtraObjectsTransforms();
				break;
			case ModAction.Painting:
				ApplyPaintStamp(stamp);
				PaintRockAndWaterTextures();
				break;
			case ModAction.RemovingExtraObjects:
				ApplyRemovingExtraObjectsStamp(stamp, ref treesRemovedForPreview, ref extraObjectsRemovedForPreview, previewOnly: false);
				break;
			case ModAction.AddingExtraObjects:
				lastPlacedExtraObject = ApplyAddExtraObjectsStamp(stamp, ref treesAddedForPreview, ref extraObjectsAddedForPreview);
				break;
			}
		}
	}

	private void ApplyAllPaths()
	{
		foreach (TerrainPath path in paths)
		{
			switch (path.pathAction)
			{
			case ModAction.LandscapeRaising:
				ApplyHeightPath(path);
				PaintRockAndWaterTextures();
				CorrectExtraObjectsTransforms();
				break;
			case ModAction.LandscapeLowering:
				ApplyHeightPath(path);
				PaintRockAndWaterTextures();
				CorrectExtraObjectsTransforms();
				break;
			case ModAction.Smoothing:
				ApplySmoothPath(path);
				PaintRockAndWaterTextures();
				CorrectExtraObjectsTransforms();
				break;
			case ModAction.Painting:
				ApplyPaintPath(path);
				PaintRockAndWaterTextures();
				break;
			case ModAction.RemovingExtraObjects:
				ApplyRemovingExtraObjectsPath(path, ref treesRemovedForPreview, ref extraObjectsRemovedForPreview, previewOnly: false);
				break;
			case ModAction.AddingExtraObjects:
				lastPlacedExtraObject = ApplyAddExtraObjectsPath(path, ref treesAddedForPreview, ref extraObjectsAddedForPreview);
				break;
			}
		}
	}

	private void PlaceProps()
	{
		UnityEngine.Object.DestroyImmediate(LevelEditorTools.PropsParent.gameObject);
		for (int i = 0; i < propsStrings.Count; i++)
		{
			string text = propsStrings[i];
			string[] array = text.Split('|');
			int num = int.Parse(array[0]);
			Prop component = UnityEngine.Object.Instantiate(editorResources.propsDictionary[num].gameObject).GetComponent<Prop>();
			component.Initialize();
			component.Deserialize(text);
			component.Highlight(on: false);
			component.transform.parent = LevelEditorTools.PropsParent;
		}
	}

	private void PlaceRoutes()
	{
		UnityEngine.Object.DestroyImmediate(LevelEditorTools.RoutesParent.gameObject);
		List<PlayerRoute> list = new List<PlayerRoute>();
		for (int i = 0; i < routeStrings.Count; i++)
		{
			PlayerRoute playerRoute = new GameObject("Route").AddComponent<PlayerRoute>();
			playerRoute.transform.parent = LevelEditorTools.RoutesParent;
			playerRoute.transform.position = Vector3.zero;
			playerRoute.InitializeLineRenderer(editorResources.routeMaterial);
			playerRoute.Deserialize(routeStrings[i]);
			playerRoute.UpdateCheckpointPrefabs();
			playerRoute.UpdateLineRenderer();
			playerRoute.BakeRoute();
			list.Add(playerRoute);
		}
	}

	public Transform[] GetSpawnPoints()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < LevelEditorTools.PropsParent.childCount; i++)
		{
			if (LevelEditorTools.PropsParent.GetChild(i).name.Contains("SpawnPoint"))
			{
				list.Add(LevelEditorTools.PropsParent.GetChild(i));
			}
		}
		return list.ToArray();
	}

	private void PaintBaseTexture()
	{
		LevelEditorTools.PaintBaseTexture(terrain, mainTextureID);
	}

	private void PaintRockAndWaterTextures()
	{
		LevelEditorTools.PaintRockAndWaterTextures(terrain, waterPlane, waterEnabled, frozenWater, editorResources.underwaterTextureID, editorResources.rockTextureID, editorResources.minRockAngle, editorResources.maxRockAngle, mainTextureID);
	}

	private void ApplyHeightStamp(TerrainStamp stamp)
	{
		LevelEditorTools.ApplyHeightStamp(terrain, stamp, defHeights, editorResources.stampTextures);
	}

	private void ApplySmoothStamp(TerrainStamp stamp)
	{
		LevelEditorTools.ApplySmoothStamp(terrain, stamp, editorResources.stampTextures);
	}

	private void ApplyPaintStamp(TerrainStamp stamp)
	{
		LevelEditorTools.ApplyPaintStamp(terrain, editorResources.stampTextures[stamp.stampTextureID], stamp.stampRotation, stamp.stampPosition, stamp.stampSize, stamp.extraInt);
	}

	private void ApplyRemovingExtraObjectsStamp(TerrainStamp stamp, ref List<TreeInstance> removedTrees, ref List<GameObject> removedExtraObjects, bool previewOnly)
	{
		LevelEditorTools.ApplyRemoveExtraObjectsStamp(terrain, stamp, seed, editorResources.stampTextures, ref removedTrees, ref removedExtraObjects, previewOnly);
	}

	private int ApplyAddExtraObjectsStamp(TerrainStamp stamp, ref List<TreeInstance> addedTrees, ref List<GameObject> addedExtraObjects)
	{
		return LevelEditorTools.ApplyAddExtraObjectsStamp(terrain, waterPlane, stamp, editorResources.stampTextures, seed, editorResources.extraObjectsDictionary, terrainSize, waterEnabled, ref addedTrees, ref addedExtraObjects, lastPlacedExtraObject);
	}

	private void ApplyHeightPath(TerrainPath path)
	{
		LevelEditorTools.ApplyHeightPath(terrain, path, defHeights, editorResources.pathPatterns);
	}

	private void ApplySmoothPath(TerrainPath path)
	{
		LevelEditorTools.ApplySmoothPath(terrain, path, editorResources.pathPatterns);
	}

	private void ApplyPaintPath(TerrainPath path)
	{
		LevelEditorTools.ApplyPaintPath(terrain, path, editorResources.pathPatterns);
	}

	private void ApplyRemovingExtraObjectsPath(TerrainPath path, ref List<TreeInstance> removedTrees, ref List<GameObject> removedExtraObjects, bool previewOnly)
	{
		LevelEditorTools.ApplyRemoveExtraObjectsPath(terrain, path, seed, editorResources.pathPatterns, ref removedTrees, ref removedExtraObjects, previewOnly);
	}

	private int ApplyAddExtraObjectsPath(TerrainPath path, ref List<TreeInstance> addedTrees, ref List<GameObject> addedExtraObjects)
	{
		return LevelEditorTools.ApplyAddExtraObjectsPath(terrain, waterPlane, path, editorResources.pathPatterns, seed, editorResources.extraObjectsDictionary, terrainSize, waterEnabled, ref addedTrees, ref addedExtraObjects, lastPlacedExtraObject);
	}

	private void CorrectExtraObjectsTransforms()
	{
		LevelEditorTools.CorrectExtraObjectsTransforms(terrain, seed);
	}

	private void CorrectCliffsPositions()
	{
		LevelEditorTools.CorrectCliffsPositions(terrain);
	}
}
