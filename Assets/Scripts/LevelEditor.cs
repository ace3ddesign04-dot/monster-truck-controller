using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LevelEditor : MonoBehaviour
{
	public delegate void CreationStepChanged();

	public delegate void TerrainGenerated();

	public delegate void StampStateChanged();

	public delegate void ModActionChanged();

	public delegate void ProcessingTerrainStarted(string text);

	public delegate void ProcessingTerrainFinished();

	public delegate void ModsResetWarning();

	public delegate void TerrainValuesRandomized();

	public delegate void ModTypeChanged();

	public delegate void PathStateChanged();

	public delegate void SelectedPropChanged();

	public delegate void PropStateChanged();

	public delegate void PlacedObjectScaleChanged(float scaleRatio);

	public delegate void RouteCreationStepChanged();

	public delegate void RouteCreated();

	public delegate void LevelUploadFinished(bool failed);

	public delegate void SelectedMudStampChanged();

	public static LevelEditor Instance;

	private LevelEditorResources editorResources;

	private LevelEditorUI ui;

	[HideInInspector]
	public LevelCreationStep levelCreationStep;

	[HideInInspector]
	public string mapName;

	[HideInInspector]
	public string mapDescription;

	[HideInInspector]
	public string mapID;

	[HideInInspector]
	public int mapRating;

	[HideInInspector]
	public bool mapUploaded;

	[HideInInspector]
	public bool mapVisible;

	[Header("Landscape")]
	public int seed;

	public float terrainSize = 500f;

	[Range(0f, 1f)]
	public float bumpsStrength = 0.01f;

	private const float tileSize = 10f;

	public bool flatCenter;

	[Header("Trees")]
	[Range(0f, 1f)]
	public float treesDensity = 0.5f;

	public bool treesByEdgesOnly;

	public ExtraObjectReference[] usedExtraObjects;

	[HideInInspector]
	public List<int> ignoredExtraObjectIDs = new List<int>();

	[HideInInspector]
	public int lastPlacedExtraObjectID;

	[Header("Water")]
	public Transform waterPlane;

	[Range(0.3f, 0.6f)]
	public float waterHeight;

	public bool waterEnabled;

	public bool frozenWater;

	[Header("Textures")]
	public int mainTextureID;

	[Header("Camera navigation")]
	public Transform cameraTarget;

	private float minDistance = 5f;

	private float maxDistance = 600f;

	private float moveSensevitity = 0.2f;

	private float rotateSensevitity = 0.2f;

	private float zoomSensevitity = 0.1f;

	private Vector3 cameraTarget_TargetPos;

	private Camera mainCamera;

	private bool doubleTouch;

	private float camX;

	private float camY;

	private float distance;

	private float distanceTarget;

	private float camXTarget;

	private float camYTarget;

	[HideInInspector]
	public TerrainModifyingType terrainModType;

	[HideInInspector]
	public ModAction modAction;

	[HideInInspector]
	[Range(0f, 1f)]
	public float modStrength;

	[HideInInspector]
	public int modPaintTextureID;

	[HideInInspector]
	public int addingExtraObjectID;

	private float[,] defHeights;

	private float[,] heightsBeforePreview;

	private float[,,] splatBeforePreview;

	private List<TreeInstance> treesAddedForPreview = new List<TreeInstance>();

	private List<TreeInstance> treesRemovedForPreview = new List<TreeInstance>();

	private List<GameObject> extraObjectsAddedForPreview = new List<GameObject>();

	private List<GameObject> extraObjectsRemovedForPreview = new List<GameObject>();

	[Header("Stamps")]
	public Projector stampProjector;

	public float stampMoveSensevitity = 1f;

	public Transform sizingButton;

	public Transform movingButton;

	[HideInInspector]
	public int stampTextureID;

	[HideInInspector]
	public List<TerrainStamp> stamps = new List<TerrainStamp>();

	[HideInInspector]
	public float stampSize;

	[HideInInspector]
	public float stampRotation;

	[HideInInspector]
	public StampState stampState;

	[HideInInspector]
	public bool movingStamp;

	[HideInInspector]
	public bool sizingStamp;

	[HideInInspector]
	public Vector3 stampPosition;

	private Vector3 stampLookPos;

	private Vector3 lastTouchPos;

	[Header("Paths")]
	public float pathWaypointsDistance;

	public int maxPathWaypoints;

	public Material pathMaterial;

	[HideInInspector]
	public float pathWidth;

	[HideInInspector]
	public List<TerrainPath> paths = new List<TerrainPath>();

	[HideInInspector]
	public PathState pathState;

	[HideInInspector]
	public Vector3 lastPathPoint;

	[HideInInspector]
	public bool draggingScreen;

	[HideInInspector]
	public int selectedPathPattern;

	private List<Vector3> pathPositions = new List<Vector3>();

	private bool drawingPath;

	private LineRenderer lineRenderer;

	private Vector3 tempPathPoint;

	[Header("Objects placement")]
	public GameObject movingPropButton;

	public GameObject sizingPropButton;

	public GameObject liftingPropButton;

	public float liftSensevitity;

	public int spawnPointPropID;

	[HideInInspector]
	public PropPlacementState propState;

	[HideInInspector]
	public bool alignBySlope;

	[HideInInspector]
	public int selectedPropID;

	[HideInInspector]
	public Prop PlacedProp;

	[HideInInspector]
	public bool movingProp;

	[HideInInspector]
	public bool sizingProp;

	[HideInInspector]
	public bool liftingProp;

	private Vector3 lastRaycastPos;

	private Vector3 placedPropLookPos;

	[Header("Route placement")]
	public GameObject movingCheckpointButton;

	[HideInInspector]
	public RouteCreationStep routeCreationStep;

	[HideInInspector]
	public int selectedRouteID;

	[HideInInspector]
	public List<PlayerRoute> routes = new List<PlayerRoute>();

	[HideInInspector]
	public bool movingCheckpoint;

	[HideInInspector]
	public int selectedCheckpointID;

	[Header("Adding mud")]
	public Color selectedMudStampColor;

	public Color deselectedMudStampColor;

	public GameObject movingMudStampButton;

	public GameObject sizingMudStampButton;

	public Vector3 mudStampLookPos;

	public float minMudStampSize;

	public float maxMudStampSize;

	public List<MudStamp> mudStamps = new List<MudStamp>();

	[HideInInspector]
	public MudStampState mudStampState;

	[HideInInspector]
	public int selectedMudStampID;

	[HideInInspector]
	public bool movingMudStamp;

	[HideInInspector]
	public bool sizingMudStamp;

	private float[,,] tempSplatMap;

	public CreationStepChanged OnCreationStepChanged;

	public TerrainGenerated OnTerrainGenerated;

	public StampStateChanged OnStampStateChanged;

	public ModActionChanged OnModActionChanged;

	public ProcessingTerrainStarted OnProcessingTerrainStarted;

	public ProcessingTerrainFinished OnProcessingTerrainFinished;

	public ModsResetWarning OnModsResetWarning;

	public TerrainValuesRandomized OnTerrainValuesRandomized;

	public ModTypeChanged OnModTypeChanged;

	public PathStateChanged OnPathStateChanged;

	public SelectedPropChanged OnSelectedPropChanged;

	public PropStateChanged OnPropStateChanged;

	public PlacedObjectScaleChanged OnPlacedObjectScaleChanged;

	public RouteCreationStepChanged OnRouteCreationStepChanged;

	public RouteCreated OnRouteCreated;

	public LevelUploadFinished OnLevelUploadFinished;

	public SelectedMudStampChanged OnSelectedMudStampChanged;

	private Terrain _cachedTerrain;

	private TerrainData _cachedTerData;

	private TerrainCollider _cachedTerCollider;

	private bool splatCached;

	public Terrain terrain
	{
		get
		{
			if (_cachedTerrain == null)
			{
				_cachedTerrain = Terrain.activeTerrain;
			}
			return _cachedTerrain;
		}
	}

	public TerrainData terData
	{
		get
		{
			if (_cachedTerData == null)
			{
				_cachedTerData = terrain.terrainData;
			}
			return _cachedTerData;
		}
	}

	public TerrainCollider terCollider
	{
		get
		{
			if (_cachedTerCollider == null)
			{
				_cachedTerCollider = terrain.GetComponent<TerrainCollider>();
			}
			return _cachedTerCollider;
		}
	}

	private int heightmapWidth => terData.heightmapResolution;

	private int alphamapWidth => terData.alphamapWidth;

	public Transform SelectedCheckpoint
	{
		get
		{
			if (selectedRouteID < 0)
			{
				return null;
			}
			if (selectedCheckpointID < 0)
			{
				return null;
			}
			return routes[selectedRouteID].checkpoints[selectedCheckpointID];
		}
	}

	public MudStamp SelectedMudStamp
	{
		get
		{
			if (selectedMudStampID >= 0)
			{
				return mudStamps[selectedMudStampID];
			}
			return null;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		editorResources = LevelEditorTools.editorResources;
	}

	private void Start()
	{
		usedExtraObjects = new ExtraObjectReference[3];
		for (int i = 0; i < usedExtraObjects.Length; i++)
		{
			usedExtraObjects[i] = new ExtraObjectReference();
			usedExtraObjects[i].arrayID = -1;
		}
		ui = GetComponent<LevelEditorUI>();
		stampSize = 35f;
		modStrength = 0.5f;
		stampTextureID = 0;
		stampRotation = 0f;
		InitializeLineRenderer();
		InitializeCamera();
		UpdateStampProjector();
		ChangeLevelCreationStep(LevelCreationStep.None);
		ChangeModType(TerrainModifyingType.Stamp);
	}

	private void Update()
	{
		DoInput();
		DoCamera();
		if (drawingPath)
		{
			DrawPath();
		}
		if (movingProp)
		{
			MoveProp();
		}
		if (sizingProp)
		{
			SizeProp();
		}
		if (movingStamp)
		{
			MoveStamp();
		}
		if (sizingStamp)
		{
			SizeStamp();
		}
		if (liftingProp)
		{
			LiftProp();
		}
		if (movingCheckpoint)
		{
			MoveCheckpoint();
		}
		if (movingMudStamp)
		{
			MoveMudStamp();
		}
		if (sizingMudStamp)
		{
			SizeMudStamp();
		}
	}

	private void ResetTerrain()
	{
		LevelEditorTools.ResetTerrain(terrain);
	}

	[ContextMenu("Upload 100 maps")]
	private void Upload100Maps()
	{
		LevelBuilder component = GetComponent<LevelBuilder>();
		for (int i = 0; i < 100; i++)
		{
			component.UploadLevel(this);
		}
	}

	public void UploadMap()
	{
		LevelBuilder component = GetComponent<LevelBuilder>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("Attach LevelBuilder to LevelEditor object!");
		}
		else
		{
			component.UploadLevel(this, LevelUploaded, LevelUploadFailed);
		}
	}

	public void LevelUploaded()
	{
		if (OnLevelUploadFinished != null)
		{
			OnLevelUploadFinished(failed: false);
		}
		mapUploaded = true;
		LevelEditorTools.AddMapToMyMaps(mapID);
		ChangeLevelCreationStep(LevelCreationStep.None);
	}

	public void LevelUploadFailed()
	{
		if (OnLevelUploadFinished != null)
		{
			OnLevelUploadFinished(failed: true);
		}
		ChangeLevelCreationStep(LevelCreationStep.None);
	}

	public void ChangeLevelCreationStep(LevelCreationStep newStep)
	{
		if (levelCreationStep == LevelCreationStep.Modifying)
		{
			RemoveStamp();
			RemovePath();
		}
		stampSize = 35f;
		stampProjector.orthographicSize = 35f;
		ClearPreviewCache();
		CheckRoutesLenghts();
		ToggleStampProjector(enable: false);
		levelCreationStep = newStep;
		SelectRoute(-1);
		SelectMudStamp(-1);
		ChangePropState(PropPlacementState.NotSelected);
		if (defHeights == null)
		{
			defHeights = terData.GetHeights(0, 0, heightmapWidth, heightmapWidth);
		}
		if (OnCreationStepChanged != null)
		{
			OnCreationStepChanged();
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

	public int CountProps()
	{
		int num = 0;
		for (int i = 0; i < LevelEditorTools.PropsParent.childCount; i++)
		{
			if (!LevelEditorTools.PropsParent.GetChild(i).name.Contains("SpawnPoint"))
			{
				num++;
			}
		}
		return num;
	}

	public void PreBakeProps()
	{
		Prop[] componentsInChildren = LevelEditorTools.PropsParent.GetComponentsInChildren<Prop>();
		foreach (Prop prop in componentsInChildren)
		{
			prop.PreBakeProp();
		}
	}

	public void CancelPreBakeProps()
	{
		Prop[] componentsInChildren = LevelEditorTools.PropsParent.GetComponentsInChildren<Prop>();
		foreach (Prop prop in componentsInChildren)
		{
			prop.CancelPreBake();
		}
	}

	public void CacheSplatMaps()
	{
		tempSplatMap = terData.GetAlphamaps(0, 0, alphamapWidth, alphamapWidth);
		splatCached = true;
	}

	public void RestoreSplatMaps()
	{
		if (splatCached)
		{
			terData.SetAlphamaps(0, 0, tempSplatMap);
			splatCached = false;
		}
	}

	private void DoInput()
	{
		if (CrossPlatformInputManager.GetButtonDown("MoveStamp"))
		{
			movingStamp = true;
			CatchLastRaycastPos();
		}
		if (CrossPlatformInputManager.GetButtonUp("MoveStamp"))
		{
			movingStamp = false;
		}
		if (CrossPlatformInputManager.GetButtonDown("SizeStamp"))
		{
			sizingStamp = true;
			CatchLastRaycastPos();
			stampLookPos = stampPosition + stampProjector.transform.up * stampProjector.orthographicSize;
		}
		if (CrossPlatformInputManager.GetButtonUp("SizeStamp"))
		{
			sizingStamp = false;
		}
		if (CrossPlatformInputManager.GetButtonUp("RemoveStamp"))
		{
			RemoveStamp();
		}
		if (CrossPlatformInputManager.GetButtonUp("RemovePath"))
		{
			RemovePath();
		}
		if (CrossPlatformInputManager.GetButtonDown("MoveProp"))
		{
			movingProp = true;
			CatchLastRaycastPos();
		}
		if (CrossPlatformInputManager.GetButtonUp("MoveProp"))
		{
			movingProp = false;
		}
		if (CrossPlatformInputManager.GetButtonDown("SizeProp"))
		{
			sizingProp = true;
			Vector3 position = PlacedProp.transform.position;
			Vector3 a = PlacedProp.transform.forward * ui.currentCircleDrawerRadius;
			Vector3 localScale = PlacedProp.transform.localScale;
			placedPropLookPos = position + a * localScale.x * PlacedProp.circleDrawerSizeMultiplier;
			CatchLastRaycastPos();
		}
		if (CrossPlatformInputManager.GetButtonUp("SizeProp"))
		{
			sizingProp = false;
		}
		if (CrossPlatformInputManager.GetButtonDown("LiftProp"))
		{
			liftingProp = true;
			CatchLastRaycastPos();
		}
		if (CrossPlatformInputManager.GetButtonUp("LiftProp"))
		{
			liftingProp = false;
		}
		if (CrossPlatformInputManager.GetButtonUp("ApplyCheckpoint"))
		{
			ApplyCheckpoint();
		}
		if (CrossPlatformInputManager.GetButtonUp("RemoveCheckpoint"))
		{
			RemoveCheckpoint();
		}
		if (CrossPlatformInputManager.GetButtonDown("MoveCheckpoint"))
		{
			movingCheckpoint = true;
			CatchLastRaycastPos();
		}
		if (CrossPlatformInputManager.GetButtonUp("MoveCheckpoint"))
		{
			movingCheckpoint = false;
		}
		if (CrossPlatformInputManager.GetButtonDown("MoveMudStamp"))
		{
			movingMudStamp = true;
			CatchLastRaycastPos();
		}
		if (CrossPlatformInputManager.GetButtonUp("MoveMudStamp"))
		{
			movingMudStamp = false;
		}
		if (CrossPlatformInputManager.GetButtonDown("SizeMudStamp"))
		{
			sizingMudStamp = true;
			CatchLastRaycastPos();
			mudStampLookPos = SelectedMudStamp.stampPosition + SelectedMudStamp.stampIndicator.transform.forward * SelectedMudStamp.stampSize;
		}
		if (CrossPlatformInputManager.GetButtonUp("SizeMudStamp"))
		{
			sizingMudStamp = false;
		}
		if (CrossPlatformInputManager.GetButtonUp("RemoveMudStamp"))
		{
			RemoveMudStamp();
		}
		if (CrossPlatformInputManager.GetButtonUp("ApplyMudStamp"))
		{
			ApplyMudStamp();
		}
		drawingPath = (levelCreationStep == LevelCreationStep.Modifying && terrainModType == TerrainModifyingType.Path && pathState == PathState.Drawing && draggingScreen);
	}

	private void CatchLastRaycastPos()
	{
		if (UnityEngine.Input.touchCount == 1)
		{
			lastTouchPos = UnityEngine.Input.GetTouch(0).position;
			Vector3 position = UnityEngine.Input.GetTouch(0).position;
			Ray ray = Camera.main.ScreenPointToRay(position);
			if (terCollider.Raycast(ray, out RaycastHit hitInfo, 10000f))
			{
				lastRaycastPos = hitInfo.point;
			}
		}
	}

	public void OnTouchTap(Vector3 pos, bool fingerMoved, bool doubleTap = false)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hitInfo;
		RaycastHit[] array;
		switch (levelCreationStep)
		{
		case LevelCreationStep.Modifying:
			if (terrainModType == TerrainModifyingType.Stamp)
			{
				if (!fingerMoved && stampState == StampState.NotPlaced && terCollider.Raycast(ray, out hitInfo, 10000f))
				{
					PlaceStamp(hitInfo.point);
					return;
				}
				break;
			}
			if (pathState == PathState.NotDrawn && !fingerMoved)
			{
				ChangePathState(PathState.Drawing);
				return;
			}
			if (pathState != PathState.Drawing)
			{
				break;
			}
			if (pathPositions.Count > 2)
			{
				ChangePathState(PathState.FinishedDrawing);
			}
			else
			{
				RemovePath();
			}
			return;
		case LevelCreationStep.PlacingObjects:
		{
			if (fingerMoved)
			{
				break;
			}
			if (propState == PropPlacementState.Selected && PlacedProp == null && Physics.Raycast(ray, out hitInfo))
			{
				PlaceProp(hitInfo.point);
				PlacedProp.ToggleExtra0(on: false);
				PlacedProp.ToggleExtra1(on: false);
				ChangePropState(PropPlacementState.Placed);
				return;
			}
			array = Physics.RaycastAll(ray);
			RaycastHit[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				RaycastHit raycastHit = array2[j];
				Prop componentInParent2 = raycastHit.collider.GetComponentInParent<Prop>();
				if (componentInParent2 != null)
				{
					if (componentInParent2.transform.parent == LevelEditorTools.ExtraObjectsParent)
					{
						ExtraObject component = componentInParent2.GetComponent<ExtraObject>();
						ignoredExtraObjectIDs.Add(component.ID);
						componentInParent2.transform.parent = LevelEditorTools.PropsParent;
						UnityEngine.Object.Destroy(component);
					}
					ChangeLevelCreationStep(LevelCreationStep.PlacingObjects);
					CatchProp(componentInParent2);
					return;
				}
			}
			break;
		}
		case LevelCreationStep.PlacingRoutes:
			if (routeCreationStep != RouteCreationStep.Selected || fingerMoved)
			{
				break;
			}
			if (doubleTap)
			{
				if (Physics.Raycast(ray, out hitInfo))
				{
					AddRouteWaypoint(hitInfo.point);
					return;
				}
			}
			else if (Physics.Raycast(ray, out hitInfo))
			{
				PlayerRouteCheckpoint componentInParent = hitInfo.collider.GetComponentInParent<PlayerRouteCheckpoint>();
				if (componentInParent != null)
				{
					SelectCheckpoint(componentInParent.checkpointID);
					return;
				}
			}
			break;
		case LevelCreationStep.AddingMud:
			if (fingerMoved)
			{
				break;
			}
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.transform.parent == LevelEditorTools.MudStampsParent)
			{
				for (int i = 0; i < mudStamps.Count; i++)
				{
					if (mudStamps[i].stampIndicator == hitInfo.collider.gameObject && selectedMudStampID != i)
					{
						SelectMudStamp(i);
						return;
					}
				}
			}
			if (doubleTap && terCollider.Raycast(ray, out hitInfo, 10000f))
			{
				AddMudStamp(hitInfo.point);
				return;
			}
			break;
		}
		if (fingerMoved || (levelCreationStep == LevelCreationStep.PlacingRoutes && routeCreationStep == RouteCreationStep.Selected))
		{
			return;
		}
		array = Physics.RaycastAll(ray);
		RaycastHit[] array3 = array;
		for (int k = 0; k < array3.Length; k++)
		{
			RaycastHit raycastHit2 = array3[k];
			Prop componentInParent3 = raycastHit2.collider.GetComponentInParent<Prop>();
			if (componentInParent3 != null)
			{
				if (componentInParent3.transform.parent == LevelEditorTools.ExtraObjectsParent)
				{
					ExtraObject component2 = componentInParent3.GetComponent<ExtraObject>();
					ignoredExtraObjectIDs.Add(component2.ID);
					componentInParent3.transform.parent = LevelEditorTools.PropsParent;
					UnityEngine.Object.Destroy(component2);
				}
				ChangeLevelCreationStep(LevelCreationStep.PlacingObjects);
				CatchProp(componentInParent3);
				return;
			}
		}
		RaycastHit[] array4 = array;
		for (int l = 0; l < array4.Length; l++)
		{
			RaycastHit raycastHit3 = array4[l];
			if (!(raycastHit3.collider.transform.parent == LevelEditorTools.MudStampsParent))
			{
				continue;
			}
			for (int m = 0; m < mudStamps.Count; m++)
			{
				if (mudStamps[m].stampIndicator == raycastHit3.collider.gameObject)
				{
					ChangeLevelCreationStep(LevelCreationStep.AddingMud);
					SelectMudStamp(m);
					return;
				}
			}
		}
	}

	private void CatchProp(Prop prop)
	{
		PlacedProp = prop;
		PlacedProp.Highlight(on: true);
		selectedPropID = prop.propID;
		if (OnSelectedPropChanged != null)
		{
			OnSelectedPropChanged();
		}
		ChangePropState(PropPlacementState.Placed);
	}

	private void MoveMudStamp()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			Vector3 vector2 = vector - lastTouchPos;
			movingMudStampButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (vector != Vector3.zero)
		{
			Ray ray = Camera.main.ScreenPointToRay(vector);
			if (terCollider.Raycast(ray, out RaycastHit hitInfo, 10000f))
			{
				Vector3 b = hitInfo.point - lastRaycastPos;
				b.y = 0f;
				Vector3 posAttempt = SelectedMudStamp.stampIndicator.transform.position + b;
				posAttempt = FilterMudStampPosition(posAttempt, selectedMudStampID);
				SelectedMudStamp.stampIndicator.transform.position = posAttempt;
				SelectedMudStamp.stampPosition = posAttempt;
				lastRaycastPos = hitInfo.point;
			}
		}
	}

	private void SizeMudStamp()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			Vector3 vector2 = vector - lastTouchPos;
			sizingMudStampButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (vector != Vector3.zero)
		{
			Ray ray = Camera.main.ScreenPointToRay(vector);
			if (terCollider.Raycast(ray, out RaycastHit hitInfo, 10000f))
			{
				Vector3 vector3 = hitInfo.point - lastRaycastPos;
				vector3.y = 0f;
				mudStampLookPos += vector3;
				lastRaycastPos = hitInfo.point;
				mudStampLookPos.y = SelectedMudStamp.stampPosition.y;
				Vector3 forward = mudStampLookPos - SelectedMudStamp.stampPosition;
				SelectedMudStamp.stampIndicator.transform.rotation = Quaternion.LookRotation(forward);
				MudStamp selectedMudStamp = SelectedMudStamp;
				Vector3 eulerAngles = SelectedMudStamp.stampIndicator.transform.eulerAngles;
				selectedMudStamp.stampRotation = eulerAngles.y;
				float value = Vector3.Distance(SelectedMudStamp.stampPosition, mudStampLookPos);
				value = Mathf.Clamp(value, minMudStampSize, maxMudStampSize);
				SelectedMudStamp.stampIndicator.transform.localScale = Vector3.one * value / 5f;
				SelectedMudStamp.stampSize = value;
				Vector3 posAttempt = SelectedMudStamp.stampPosition;
				posAttempt = FilterMudStampPosition(posAttempt, selectedMudStampID);
				SelectedMudStamp.stampPosition = posAttempt;
				SelectedMudStamp.stampIndicator.transform.position = posAttempt;
			}
		}
	}

	private void AddMudStamp(Vector3 pos)
	{
		if (mudStamps.Count < 3)
		{
			MudStamp mudStamp = new MudStamp();
			mudStamp.stampPosition = pos;
			mudStamp.stampRotation = 0f;
			mudStamp.stampSize = 20f;
			mudStamp.stampTextureID = 0;
			mudStamp.mudDepth = 0.3f;
			mudStamp.mudViscosity = 3f;
			mudStamp.stampIndicator = LevelEditorTools.CreateMudIndicator(mudStamp, terrain);
			mudStamps.Add(mudStamp);
			SelectMudStamp(mudStamps.Count - 1);
			pos = FilterMudStampPosition(pos, mudStamps.Count - 1);
			mudStamp.stampIndicator.transform.position = pos;
		}
	}

	public void SelectMudStamp(int id)
	{
		selectedMudStampID = id;
		for (int i = 0; i < mudStamps.Count; i++)
		{
			mudStamps[i].stampIndicator.GetComponent<Renderer>().material.SetColor("_BaseColor", (i != selectedMudStampID) ? deselectedMudStampColor : selectedMudStampColor);
		}
		mudStampState = ((id != -1) ? MudStampState.Selected : MudStampState.NotSelected);
		if (OnSelectedMudStampChanged != null)
		{
			OnSelectedMudStampChanged();
		}
	}

	public void ChangeMudStampPattern(int newPatternID)
	{
		if (SelectedMudStamp != null)
		{
			SelectedMudStamp.stampTextureID = newPatternID;
			Material material = SelectedMudStamp.stampIndicator.GetComponent<MeshRenderer>().material;
			material.SetTexture("_MainTex", editorResources.mudStampTextures[newPatternID]);
		}
	}

	public void ChangeMudDepth(float depth)
	{
		if (SelectedMudStamp != null)
		{
			SelectedMudStamp.mudDepth = depth;
		}
	}

	public void ChangeMudViscosity(float viscosity)
	{
		if (SelectedMudStamp != null)
		{
			SelectedMudStamp.mudViscosity = viscosity;
		}
	}

	public void RemoveAllMudStamps()
	{
		mudStamps.Clear();
		UnityEngine.Object.DestroyImmediate(LevelEditorTools.MudStampsParent.gameObject);
		SelectMudStamp(-1);
	}

	public void ApplyMudStamp()
	{
		SelectMudStamp(-1);
	}

	public void RemoveMudStamp()
	{
		UnityEngine.Object.DestroyImmediate(SelectedMudStamp.stampIndicator);
		mudStamps.RemoveAt(selectedMudStampID);
		SelectMudStamp(-1);
	}

	public Vector3 FilterMudStampPosition(Vector3 posAttempt, int mudStampID)
	{
		for (int i = 0; i < mudStamps.Count; i++)
		{
			if (i != mudStampID)
			{
				float num = mudStamps[i].boundsRadius + mudStamps[mudStampID].boundsRadius;
				float num2 = Vector3.Distance(mudStamps[i].stampPosition, posAttempt);
				if (num2 < num)
				{
					Vector3 vector = posAttempt - mudStamps[i].stampPosition;
					posAttempt = mudStamps[i].stampPosition + vector.normalized * num;
				}
			}
		}
		posAttempt.y = terrain.SampleHeight(posAttempt);
		return posAttempt;
	}

	private void InitializeLineRenderer()
	{
		lineRenderer = new GameObject("Line renderer").AddComponent<LineRenderer>();
		lineRenderer.useWorldSpace = true;
		lineRenderer.material = pathMaterial;
		lineRenderer.alignment = LineAlignment.TransformZ;
		lineRenderer.widthMultiplier = pathWidth;
		lineRenderer.positionCount = 0;
		lineRenderer.transform.eulerAngles = new Vector3(90f, 0f, 0f);
		lineRenderer.numCornerVertices = 5;
	}

	private void DrawPath()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
		}
		if (vector == Vector3.zero)
		{
			return;
		}
		bool flag = pathPositions.Count == 0;
		Ray ray = mainCamera.ScreenPointToRay(vector);
		if (terCollider.Raycast(ray, out RaycastHit hitInfo, 10000f))
		{
			tempPathPoint = hitInfo.point;
			if (flag)
			{
				pathPositions.Add(tempPathPoint);
				pathPositions.Add(tempPathPoint);
				lastPathPoint = tempPathPoint;
				lineRenderer.positionCount = 2;
				lineRenderer.SetPosition(lineRenderer.positionCount - 2, tempPathPoint);
			}
			else
			{
				ref Vector3 reference = ref tempPathPoint;
				Vector3 vector2 = pathPositions[0];
				reference.y = vector2.y;
				float num = Vector3.Distance(tempPathPoint, lastPathPoint);
				if (num > pathWaypointsDistance)
				{
					AddPathPoint(tempPathPoint);
				}
			}
			lineRenderer.SetPosition(lineRenderer.positionCount - 1, tempPathPoint);
		}
		if (pathPositions.Count > maxPathWaypoints)
		{
			ChangePathState(PathState.FinishedDrawing);
		}
	}

	private void AddPathPoint(Vector3 point)
	{
		if (pathPositions.Count > 0)
		{
			Vector3 vector = pathPositions[0];
			point.y = vector.y;
		}
		pathPositions.Add(point);
		lastPathPoint = point;
		lineRenderer.positionCount++;
		lineRenderer.SetPosition(lineRenderer.positionCount - 2, point);
	}

	private void ChangePathState(PathState newState)
	{
		pathState = newState;
		if (OnPathStateChanged != null)
		{
			OnPathStateChanged();
		}
	}

	private void RemovePath()
	{
		CancelModPreviewChanges();
		pathPositions.Clear();
		lineRenderer.positionCount = 0;
		ChangePathState(PathState.NotDrawn);
	}

	public void UpdatePathSettings()
	{
		lineRenderer.widthMultiplier = pathWidth;
		lineRenderer.material.SetTexture("_MainTex", editorResources.pathPatterns[selectedPathPattern]);
	}

	public void PreviewPath()
	{
		StartCoroutine(PreviewPathCor());
	}

	private IEnumerator PreviewPathCor()
	{
		if (OnProcessingTerrainStarted != null)
		{
			OnProcessingTerrainStarted("Building preview...");
		}
		yield return null;
		CancelModPreviewChanges();
		if (heightsBeforePreview == null)
		{
			heightsBeforePreview = terData.GetHeights(0, 0, heightmapWidth, heightmapWidth);
		}
		if (splatBeforePreview == null)
		{
			splatBeforePreview = terData.GetAlphamaps(0, 0, alphamapWidth, alphamapWidth);
		}
		ApplyPath(previewOnly: true);
		yield return null;
		if (OnProcessingTerrainFinished != null)
		{
			OnProcessingTerrainFinished();
		}
	}

	public void ApplyAndSavePath()
	{
		StartCoroutine(ApplyAndSavePathCor());
	}

	private IEnumerator ApplyAndSavePathCor()
	{
		if (OnProcessingTerrainStarted != null)
		{
			OnProcessingTerrainStarted("Applying path...");
		}
		yield return null;
		CancelModPreviewChanges();
		ApplyPath(previewOnly: false);
		RemovePath();
		yield return null;
		if (OnProcessingTerrainFinished != null)
		{
			OnProcessingTerrainFinished();
		}
	}

	private void ApplyPath(bool previewOnly)
	{
		TerrainPath terrainPath = new TerrainPath();
		terrainPath.pathAction = modAction;
		terrainPath.pathPositions = new List<Vector3>(pathPositions);
		terrainPath.pathStrength = modStrength;
		terrainPath.pathWidth = pathWidth;
		terrainPath.pathPattern = selectedPathPattern;
		if (modAction == ModAction.Painting)
		{
			terrainPath.extraInt = modPaintTextureID;
		}
		if (modAction == ModAction.AddingExtraObjects)
		{
			terrainPath.extraInt = addingExtraObjectID;
		}
		if (!previewOnly)
		{
			paths.Add(terrainPath);
		}
		switch (modAction)
		{
		case ModAction.LandscapeRaising:
			ApplyHeightPath(terrainPath);
			PaintRockAndWaterTextures();
			CorrectExtraObjectsTransforms();
			CorrectCliffsPositions();
			break;
		case ModAction.LandscapeLowering:
			ApplyHeightPath(terrainPath);
			PaintRockAndWaterTextures();
			CorrectExtraObjectsTransforms();
			CorrectCliffsPositions();
			break;
		case ModAction.Smoothing:
			ApplySmoothPath(terrainPath);
			PaintRockAndWaterTextures();
			CorrectExtraObjectsTransforms();
			CorrectCliffsPositions();
			break;
		case ModAction.Painting:
			ApplyPaintPath(terrainPath);
			PaintRockAndWaterTextures();
			break;
		case ModAction.RemovingExtraObjects:
			ApplyRemovingExtraObjectsPath(terrainPath, ref treesRemovedForPreview, ref extraObjectsRemovedForPreview, previewOnly);
			break;
		case ModAction.AddingExtraObjects:
			lastPlacedExtraObjectID = ApplyAddExtraObjectsPath(terrainPath, ref treesAddedForPreview, ref extraObjectsAddedForPreview);
			break;
		}
		if (!previewOnly)
		{
			ClearPreviewCache();
		}
	}

	private void SizeStamp()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			Vector3 vector2 = vector - lastTouchPos;
			sizingButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (vector != Vector3.zero)
		{
			Ray ray = Camera.main.ScreenPointToRay(vector);
			if (terCollider.Raycast(ray, out RaycastHit hitInfo, 10000f))
			{
				Vector3 vector3 = hitInfo.point - lastRaycastPos;
				vector3.y = 0f;
				stampLookPos += vector3;
				lastRaycastPos = hitInfo.point;
				Vector3 upwards = stampLookPos - stampPosition;
				stampProjector.transform.rotation = Quaternion.LookRotation(Vector3.down, upwards);
				Vector3 eulerAngles = stampProjector.transform.eulerAngles;
				stampRotation = eulerAngles.y;
				float orthographicSize = Vector3.Distance(stampPosition, stampLookPos);
				stampProjector.orthographicSize = orthographicSize;
				stampSize = orthographicSize;
			}
		}
	}

	private void MoveStamp()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			Vector3 vector2 = vector - lastTouchPos;
			movingButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (vector != Vector3.zero)
		{
			Ray ray = Camera.main.ScreenPointToRay(vector);
			if (terCollider.Raycast(ray, out RaycastHit hitInfo, 10000f))
			{
				Vector3 b = hitInfo.point - lastRaycastPos;
				b.y = 0f;
				Vector3 pos = stampPosition + b;
				PlaceStamp(pos);
				lastRaycastPos = hitInfo.point;
			}
		}
	}

	public void ChangeModType(TerrainModifyingType newType)
	{
		terrainModType = newType;
		RemoveStamp();
		RemovePath();
		if (OnModTypeChanged != null)
		{
			OnModTypeChanged();
		}
	}

	private void ChangeStampState(StampState newState)
	{
		stampState = newState;
		ToggleStampProjector(stampState == StampState.Placed);
		UpdateStampProjector();
		if (OnStampStateChanged != null)
		{
			OnStampStateChanged();
		}
	}

	public void UpdateStampProjector()
	{
		stampProjector.material.SetTexture("_ShadowTex", editorResources.stampTextures[stampTextureID]);
	}

	private void ToggleStampProjector(bool enable)
	{
		stampProjector.gameObject.SetActive(enable);
	}

	private void PlaceStamp(Vector3 pos)
	{
		stampPosition = pos;
		stampProjector.transform.position = stampPosition + Vector3.up * 20f;
		ChangeStampState(StampState.Placed);
	}

	public void RemoveStamp()
	{
		ChangeStampState(StampState.NotPlaced);
		CancelModPreviewChanges();
	}

	public void ChangeModAction(ModAction newAction)
	{
		modAction = newAction;
		if (OnModActionChanged != null)
		{
			OnModActionChanged();
		}
	}

	public void ChangeModPaintTextureID(int newID)
	{
		modPaintTextureID = newID;
	}

	public void ApplyAndSaveStamp()
	{
		StartCoroutine(ApplyAndSaveStampCor());
	}

	private IEnumerator ApplyAndSaveStampCor()
	{
		if (OnProcessingTerrainStarted != null)
		{
			OnProcessingTerrainStarted("Applying stamp...");
		}
		yield return null;
		RemoveStamp();
		ApplyStamp(previewOnly: false);
		yield return null;
		if (OnProcessingTerrainFinished != null)
		{
			OnProcessingTerrainFinished();
		}
	}

	public void PreviewStamp()
	{
		StartCoroutine(PreviewStampCor());
	}

	private IEnumerator PreviewStampCor()
	{
		if (OnProcessingTerrainStarted != null)
		{
			OnProcessingTerrainStarted("Building preview...");
		}
		yield return null;
		CancelModPreviewChanges();
		if (heightsBeforePreview == null)
		{
			heightsBeforePreview = terData.GetHeights(0, 0, heightmapWidth, heightmapWidth);
		}
		if (splatBeforePreview == null)
		{
			splatBeforePreview = terData.GetAlphamaps(0, 0, alphamapWidth, alphamapWidth);
		}
		ApplyStamp(previewOnly: true);
		yield return null;
		if (OnProcessingTerrainFinished != null)
		{
			OnProcessingTerrainFinished();
		}
	}

	public void CancelModPreviewChanges()
	{
		if (heightsBeforePreview != null)
		{
			terData.SetHeights(0, 0, heightsBeforePreview);
		}
		if (splatBeforePreview != null)
		{
			terData.SetAlphamaps(0, 0, splatBeforePreview);
		}
		if (extraObjectsAddedForPreview.Count > 0)
		{
			lastPlacedExtraObjectID -= extraObjectsAddedForPreview.Count;
			foreach (GameObject item in extraObjectsAddedForPreview)
			{
				UnityEngine.Object.Destroy(item);
			}
		}
		if (extraObjectsRemovedForPreview.Count > 0)
		{
			foreach (GameObject item2 in extraObjectsRemovedForPreview)
			{
				if (item2 != null)
				{
					item2.SetActive(value: true);
				}
			}
		}
		List<TreeInstance> list = new List<TreeInstance>(terData.treeInstances);
		UnityEngine.Debug.Log("Trees:" + list.Count);
		UnityEngine.Debug.Log("Added trees:" + treesAddedForPreview.Count);
		if (treesAddedForPreview.Count > 0)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < treesAddedForPreview.Count; j++)
				{
					if (i < list.Count && i >= 0)
					{
						TreeInstance treeInstance = list[i];
						Vector3 position = treeInstance.position;
						position.y = 0f;
						TreeInstance treeInstance2 = treesAddedForPreview[j];
						Vector3 position2 = treeInstance2.position;
						position2.y = 0f;
						if (Vector3.Distance(position, position2) < 0.001f)
						{
							list.RemoveAt(i);
							i--;
							UnityEngine.Debug.Log("Tree removed");
						}
					}
				}
			}
		}
		if (treesRemovedForPreview.Count > 0)
		{
			for (int k = 0; k < treesRemovedForPreview.Count; k++)
			{
				list.Add(treesRemovedForPreview[k]);
			}
		}
		terData.treeInstances = list.ToArray();
		treesAddedForPreview.Clear();
		treesRemovedForPreview.Clear();
		extraObjectsAddedForPreview.Clear();
		extraObjectsRemovedForPreview.Clear();
	}

	private void ClearPreviewCache()
	{
		heightsBeforePreview = null;
		splatBeforePreview = null;
		treesAddedForPreview.Clear();
		treesRemovedForPreview.Clear();
		extraObjectsAddedForPreview.Clear();
		extraObjectsRemovedForPreview.Clear();
	}

	private void ApplyStamp(bool previewOnly)
	{
		TerrainStamp terrainStamp = new TerrainStamp();
		terrainStamp.stampAction = modAction;
		TerrainStamp terrainStamp2 = terrainStamp;
		Vector3 position = stampProjector.transform.position;
		float x = position.x;
		Vector3 position2 = stampProjector.transform.position;
		terrainStamp2.stampPosition = new Vector2(x, position2.z);
		terrainStamp.stampTextureID = stampTextureID;
		terrainStamp.stampRotation = stampRotation;
		terrainStamp.stampSize = stampSize;
		terrainStamp.stampStrength = modStrength;
		if (modAction == ModAction.Painting)
		{
			terrainStamp.extraInt = modPaintTextureID;
		}
		if (modAction == ModAction.AddingExtraObjects)
		{
			terrainStamp.extraInt = addingExtraObjectID;
		}
		if (!previewOnly)
		{
			stamps.Add(terrainStamp);
			ClearPreviewCache();
		}
		switch (modAction)
		{
		case ModAction.LandscapeRaising:
			ApplyHeightStamp(terrainStamp);
			PaintRockAndWaterTextures();
			CorrectExtraObjectsTransforms();
			CorrectCliffsPositions();
			break;
		case ModAction.LandscapeLowering:
			ApplyHeightStamp(terrainStamp);
			PaintRockAndWaterTextures();
			CorrectExtraObjectsTransforms();
			CorrectCliffsPositions();
			break;
		case ModAction.Smoothing:
			ApplySmoothStamp(terrainStamp);
			PaintRockAndWaterTextures();
			CorrectExtraObjectsTransforms();
			CorrectCliffsPositions();
			break;
		case ModAction.Painting:
			ApplyPaintStamp(terrainStamp);
			PaintRockAndWaterTextures();
			break;
		case ModAction.RemovingExtraObjects:
			ApplyRemovingExtraObjectsStamp(terrainStamp, ref treesRemovedForPreview, ref extraObjectsRemovedForPreview, previewOnly);
			break;
		case ModAction.AddingExtraObjects:
			lastPlacedExtraObjectID = ApplyAddExtraObjectsStamp(terrainStamp, ref treesAddedForPreview, ref extraObjectsAddedForPreview);
			break;
		}
		if (!previewOnly)
		{
			ClearPreviewCache();
		}
	}

	private void LiftProp()
	{
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			vector2 = vector - lastTouchPos;
			liftingPropButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (!(vector == Vector3.zero))
		{
			PlacedProp.currentLift += vector2.magnitude * liftSensevitity * Mathf.Sign(vector2.y);
			PlacedProp.currentLift = Mathf.Clamp(PlacedProp.currentLift, PlacedProp.minLift, PlacedProp.maxLift);
			PlaceProp(PlacedProp.transform.position);
		}
	}

	private void MoveProp()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			Vector3 vector2 = vector - lastTouchPos;
			movingPropButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (!(vector == Vector3.zero))
		{
			Ray ray = Camera.main.ScreenPointToRay(vector);
			if (terCollider.Raycast(ray, out RaycastHit hitInfo, 100000f))
			{
				Vector3 b = hitInfo.point - lastRaycastPos;
				Vector3 vector3 = PlacedProp.transform.position + b;
				vector3.y = terrain.SampleHeight(vector3);
				PlaceProp(vector3);
				lastRaycastPos = hitInfo.point;
			}
		}
	}

	private void SizeProp()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			Vector3 vector2 = vector - lastTouchPos;
			sizingPropButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (!(vector != Vector3.zero))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(vector);
		if (terCollider.Raycast(ray, out RaycastHit hitInfo, 10000f))
		{
			PlacementSettings component = PlacedProp.GetComponent<PlacementSettings>();
			Vector3 vector3 = hitInfo.point - lastRaycastPos;
			vector3.y = 0f;
			placedPropLookPos += vector3;
			lastRaycastPos = hitInfo.point;
			PlacedProp.transform.rotation = Quaternion.LookRotation(placedPropLookPos - PlacedProp.transform.position, PlacedProp.transform.up);
			float num = Vector3.Distance(placedPropLookPos, PlacedProp.transform.position);
			float value = num / ui.currentCircleDrawerRadius / PlacedProp.circleDrawerSizeMultiplier;
			value = Mathf.Clamp(value, PlacedProp.minScale, PlacedProp.maxScale);
			float num2 = value / PlacedProp.defaultScale;
			num2 = Mathf.Round(num2 * 10f) / 10f;
			PlacedProp.transform.localScale = Vector3.one * num2 * PlacedProp.defaultScale;
			if (OnPlacedObjectScaleChanged != null)
			{
				OnPlacedObjectScaleChanged(num2);
			}
			PlaceProp(PlacedProp.transform.position);
		}
	}

	public void ResetPropScale()
	{
		float defaultScale = PlacedProp.defaultScale;
		PlacedProp.transform.localScale = Vector3.one * defaultScale;
		if (OnPlacedObjectScaleChanged != null)
		{
			OnPlacedObjectScaleChanged(defaultScale / PlacedProp.defaultScale);
		}
	}

	public void RemoveProp()
	{
		if (!(PlacedProp == null))
		{
			PlacedProp.ResetSnapping();
			UnityEngine.Object.Destroy(PlacedProp.gameObject);
			SelectProp(-1);
			ChangePropState(PropPlacementState.NotSelected);
		}
	}

	public void SetAlignBySlope(bool align)
	{
		alignBySlope = align;
		if (PlacedProp != null)
		{
			PlaceProp(PlacedProp.transform.position);
		}
	}

	public void SelectProp(int propID)
	{
		selectedPropID = propID;
		ChangePropState(PropPlacementState.Selected);
		if (OnSelectedPropChanged != null)
		{
			OnSelectedPropChanged();
		}
	}

	private void ResetSelectedProp()
	{
		selectedPropID = -1;
		if (OnSelectedPropChanged != null)
		{
			OnSelectedPropChanged();
		}
	}

	public void ChangePropState(PropPlacementState newState)
	{
		propState = newState;
		if (propState == PropPlacementState.NotSelected || propState == PropPlacementState.Selected)
		{
			if (PlacedProp != null)
			{
				PlacedProp.Highlight(on: false);
			}
			PlacedProp = null;
		}
		if (OnPropStateChanged != null)
		{
			OnPropStateChanged();
		}
	}

	private void PlaceProp(Vector3 pos)
	{
		ChangePropState(PropPlacementState.Placed);
		if (PlacedProp == null)
		{
			PlacedProp = UnityEngine.Object.Instantiate(editorResources.propsDictionary[selectedPropID].gameObject, pos, editorResources.propsDictionary[selectedPropID].transform.rotation, LevelEditorTools.PropsParent).GetComponent<Prop>();
			PlacedProp.propID = selectedPropID;
		}
		PlacedProp.Highlight(on: true);
		pos.y = terrain.SampleHeight(pos) + PlacedProp.currentLift;
		if (alignBySlope)
		{
			LevelEditorTools.AlignByNormal(terrain, PlacedProp.transform, PlacedProp.frontSupport, PlacedProp.rearSupport);
		}
		else
		{
			PlacedProp.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(PlacedProp.transform.forward, Vector3.up), Vector3.up);
		}
		if (PlacedProp.DoSnapping() && PlacedProp.snapPosition)
		{
			Vector3 position = PlacedProp.transform.position;
			pos.y = position.y;
		}
		PlacedProp.transform.position = pos;
		PlacedProp.DoSnapping();
	}

	public void ApplyProp()
	{
		if (PlacedProp != null)
		{
			PlacedProp.Highlight(on: false);
		}
		ChangePropState(PropPlacementState.NotSelected);
		ResetSelectedProp();
	}

	public void DuplicateProp()
	{
		int propID = PlacedProp.propID;
		bool extra0Enabled = PlacedProp.extra0Enabled;
		bool extra1Enabled = PlacedProp.extra1Enabled;
		float currentLift = PlacedProp.currentLift;
		Vector3 localScale = PlacedProp.transform.localScale;
		Quaternion rotation = PlacedProp.transform.rotation;
		Vector3 vector = PlacedProp.transform.position + PlacedProp.transform.forward * 20f;
		SnapPoint snapPoint = PlacedProp.ClosestFreeSnapPoint(vector);
		int num = -1;
		if (snapPoint != null)
		{
			Vector3 position = snapPoint.transform.position;
			int suitableSnapPointID = PlacedProp.GetSuitableSnapPointID(snapPoint);
			num = suitableSnapPointID;
		}
		ApplyProp();
		SelectProp(propID);
		PlaceProp(vector);
		PlacedProp.Initialize();
		PlacedProp.currentLift = currentLift;
		PlacedProp.transform.localScale = localScale;
		PlacedProp.transform.rotation = rotation;
		PlacedProp.ToggleExtra0(extra0Enabled);
		PlacedProp.ToggleExtra1(extra1Enabled);
		if (num != -1)
		{
			Vector3 vector2 = snapPoint.transform.InverseTransformPoint(PlacedProp.snapPoints[num].transform.position);
			vector2 = PlacedProp.snapPoints[num].transform.position - snapPoint.transform.position;
			PlacedProp.transform.position -= vector2;
		}
		PlaceProp(PlacedProp.transform.position);
		ChangePropState(PropPlacementState.Placed);
	}

	private void MoveCheckpoint()
	{
		Vector3 vector = Vector3.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			vector = UnityEngine.Input.GetTouch(0).position;
			Vector3 vector2 = vector - lastTouchPos;
			movingCheckpointButton.transform.position += vector2;
			lastTouchPos = vector;
		}
		if (vector == Vector3.zero)
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(vector);
		RaycastHit[] array = (from h in Physics.RaycastAll(ray)
			orderby h.distance
			select h).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i].collider.GetComponentInParent<PlayerRoute>() != null))
			{
				Vector3 b = array[i].point - lastRaycastPos;
				Vector3 position = SelectedCheckpoint.transform.position + b;
				SelectedCheckpoint.transform.position = position;
				routes[selectedRouteID].UpdateLineRenderer();
				routes[selectedRouteID].AlignCheckpoints();
				lastRaycastPos = array[i].point;
				break;
			}
		}
		routes[selectedRouteID].routeID = LevelEditorTools.RandomName();
	}

	public void ChangeRouteCreationStep(RouteCreationStep newStep)
	{
		routeCreationStep = newStep;
		if (OnRouteCreationStepChanged != null)
		{
			OnRouteCreationStepChanged();
		}
	}

	public void CreateNewRoute()
	{
		PlayerRoute playerRoute = new GameObject("Route").AddComponent<PlayerRoute>();
		playerRoute.routeName = "New route";
		playerRoute.transform.parent = LevelEditorTools.RoutesParent;
		playerRoute.transform.position = Vector3.zero;
		playerRoute.InitializeLineRenderer(editorResources.routeMaterial);
		playerRoute.routeID = LevelEditorTools.RandomName();
		routes.Add(playerRoute);
		SelectRoute(routes.Count - 1);
		if (OnRouteCreated != null)
		{
			OnRouteCreated();
		}
	}

	public void AddRouteWaypoint(Vector3 pos)
	{
		routes[selectedRouteID].AddCheckpoint(pos);
		routes[selectedRouteID].UpdateCheckpointPrefabs();
		routes[selectedRouteID].routeID = LevelEditorTools.RandomName();
	}

	public void SelectRoute(int routeID)
	{
		selectedRouteID = routeID;
		for (int i = 0; i < routes.Count; i++)
		{
			routes[i].UpdateCheckpointPrefabs();
			if (i == selectedRouteID)
			{
				routes[i].UnBakeRoute();
			}
			else
			{
				routes[i].BakeRoute();
			}
		}
		if (routeID != -1)
		{
			ChangeRouteCreationStep(RouteCreationStep.Selected);
		}
		else
		{
			ChangeRouteCreationStep(RouteCreationStep.None);
		}
	}

	public void CheckRoutesLenghts()
	{
		for (int i = 0; i < routes.Count; i++)
		{
			if (routes[i].checkpoints.Count < 2)
			{
				UnityEngine.Object.Destroy(routes[i].gameObject);
				routes.RemoveAt(i);
			}
		}
	}

	public void ChangeCurrentRouteName(string name)
	{
		routes[selectedRouteID].routeName = name;
	}

	public void RemoveCurrentRoute()
	{
		UnityEngine.Object.Destroy(routes[selectedRouteID].gameObject);
		routes.RemoveAt(selectedRouteID);
		SelectRoute(-1);
	}

	public void ApplyRoute()
	{
		SelectRoute(-1);
	}

	public void SelectCheckpoint(int id)
	{
		selectedCheckpointID = id;
		ChangeRouteCreationStep(RouteCreationStep.ModifyingCheckpoint);
	}

	public void ApplyCheckpoint()
	{
		ChangeRouteCreationStep(RouteCreationStep.Selected);
	}

	public void RemoveCheckpoint()
	{
		UnityEngine.Object.DestroyImmediate(routes[selectedRouteID].checkpoints[selectedCheckpointID].gameObject);
		routes[selectedRouteID].checkpoints.RemoveAt(selectedCheckpointID);
		routes[selectedRouteID].UpdateCheckpointPrefabs();
		routes[selectedRouteID].UpdateLineRenderer();
		ChangeRouteCreationStep(RouteCreationStep.Selected);
		routes[selectedRouteID].routeID = LevelEditorTools.RandomName();
	}

	private void InitializeCamera()
	{
		mainCamera = Camera.main;
		cameraTarget_TargetPos = terrain.GetPosition() + terrain.terrainData.size / 2f;
		cameraTarget.position = cameraTarget_TargetPos;
		camYTarget = (camY = 45f);
		distance = (distanceTarget = maxDistance);
	}

	private void DoCamera()
	{
		doubleTouch = (UnityEngine.Input.touchCount == 2);
		if (!movingStamp && !drawingPath)
		{
			if (doubleTouch)
			{
				camXTarget += CrossPlatformInputManager.GetAxis("Drag X") * rotateSensevitity;
				camYTarget -= CrossPlatformInputManager.GetAxis("Drag Y") * rotateSensevitity;
				distanceTarget -= CrossPlatformInputManager.GetAxis("Zoom") * zoomSensevitity;
			}
			else
			{
				cameraTarget_TargetPos -= mainCamera.transform.right * CrossPlatformInputManager.GetAxis("Drag X") * moveSensevitity * distanceTarget / 50f;
				cameraTarget_TargetPos -= Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized * CrossPlatformInputManager.GetAxis("Drag Y") * moveSensevitity * distanceTarget / 50f;
			}
		}
		distanceTarget = Mathf.Clamp(distanceTarget, minDistance, maxDistance);
		distance = Mathf.Lerp(distance, distanceTarget, Time.deltaTime * 5f);
		camYTarget = Mathf.Clamp(camYTarget, 1f, 89f);
		cameraTarget_TargetPos.y = terrain.SampleHeight(cameraTarget.transform.position);
		cameraTarget.position = Vector3.Lerp(cameraTarget.position, cameraTarget_TargetPos, Time.deltaTime * 8f);
		camX = Mathf.Lerp(camX, camXTarget, Time.deltaTime * 8f);
		camY = Mathf.Lerp(camY, camYTarget, Time.deltaTime * 8f);
		Quaternion quaternion = Quaternion.Euler(camY, camX, 0f);
		Vector3 position = quaternion * new Vector3(0f, 0f, 0f - distance) + cameraTarget.position;
		mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, quaternion, Time.deltaTime * 8f);
		mainCamera.transform.position = position;
		mainCamera.transform.LookAt(cameraTarget);
	}

	[ContextMenu("Generate random terrain")]
	public void GenerateRandomTerrain()
	{
		terrainSize = UnityEngine.Random.Range(300, 1000);
		seed = UnityEngine.Random.Range(0, 10000);
		bumpsStrength = UnityEngine.Random.Range(0.3f, 1f);
		flatCenter = (UnityEngine.Random.Range(0f, 1f) < 0.5f);
		treesDensity = UnityEngine.Random.Range(0f, 1f);
		treesByEdgesOnly = true;
		mainTextureID = UnityEngine.Random.Range(0, 3);
		waterEnabled = (UnityEngine.Random.Range(0f, 1f) < 0.2f);
		waterHeight = UnityEngine.Random.Range(0.45f, 0.52f);
		frozenWater = (UnityEngine.Random.Range(0f, 1f) < 0.3f);
		for (int i = 0; i < usedExtraObjects.Length; i++)
		{
			usedExtraObjects[i].arrayID = UnityEngine.Random.Range(-1, editorResources.extraObjectsDictionary.Length);
			usedExtraObjects[i].density = UnityEngine.Random.Range(0f, 1f);
			usedExtraObjects[i].onlyByEdges = (UnityEngine.Random.Range(0f, 1f) < 0.5f);
		}
		if (OnTerrainValuesRandomized != null)
		{
			OnTerrainValuesRandomized();
		}
		GenerateTerrain(dontShowNotification: false);
	}

	public void RemoveAllModsAndProceedGeneratingTerrain()
	{
		stamps.Clear();
		paths.Clear();
		routes.Clear();
		ignoredExtraObjectIDs.Clear();
		mudStamps.Clear();
		UnityEngine.Object.DestroyImmediate(LevelEditorTools.PropsParent.gameObject);
		UnityEngine.Object.DestroyImmediate(LevelEditorTools.RoutesParent.gameObject);
		UnityEngine.Object.DestroyImmediate(LevelEditorTools.MudStampsParent.gameObject);
		GenerateTerrain(dontShowNotification: false);
	}

	public void GenerateTerrain(bool dontShowNotification)
	{
		StartCoroutine(GeneratingTerrainCor(dontShowNotification));
	}

	private IEnumerator GeneratingTerrainCor(bool dontShowNotification)
	{
		if (!dontShowNotification && (stamps.Count > 0 || paths.Count > 0 || routes.Count > 0 || ignoredExtraObjectIDs.Count > 0 || LevelEditorTools.PropsParent.childCount > 0))
		{
			if (OnModsResetWarning != null)
			{
				OnModsResetWarning();
			}
			yield break;
		}
		if (OnProcessingTerrainStarted != null)
		{
			OnProcessingTerrainStarted("Generating terrain...");
		}
		yield return null;
		float startTime = Time.realtimeSinceStartup;
		Vector3 a = cameraTarget_TargetPos - terrain.GetPosition();
		Vector3 size = terData.size;
		Vector3 cameraTargetRelativePos = a / size.x;
		ResetTerrain();
		GenerateStampBasedTerrain();
		ApplyWater();
		PlaceTrees();
		lastPlacedExtraObjectID = PlaceExtraObjects();
		PlaceCliffs();
		PaintBaseTexture();
		PaintRockAndWaterTextures();
		defHeights = terData.GetHeights(0, 0, heightmapWidth, heightmapWidth);
		if (Application.isPlaying)
		{
			Vector3 position = terrain.GetPosition();
			Vector3 a2 = cameraTargetRelativePos;
			Vector3 size2 = terData.size;
			cameraTarget_TargetPos = position + a2 * size2.x;
		}
		float endTime = Time.realtimeSinceStartup;
		float functionTime = endTime - startTime;
		UnityEngine.Debug.Log("Terrain generated for " + functionTime);
		yield return null;
		if (OnProcessingTerrainFinished != null)
		{
			OnProcessingTerrainFinished();
		}
		if (OnTerrainGenerated != null)
		{
			OnTerrainGenerated();
		}
	}

	private void GeneratePerlinTerrain()
	{
		LevelEditorTools.GeneratePerlinTerrain(terrain, seed, terrainSize, flatCenter, bumpsStrength);
	}

	private void GenerateStampBasedTerrain()
	{
		LevelEditorTools.GenerateStampBasedTerrain(terrain, editorResources.terrainGenerationStamps, seed, terrainSize, flatCenter, bumpsStrength);
	}

	private void ApplyWater()
	{
		LevelEditorTools.ApplyWater(terrain, waterPlane, waterEnabled, terrainSize, waterHeight, frozenWater, editorResources.frozenWaterMaterial, editorResources.waterMaterial);
	}

	private void PlaceTrees()
	{
		LevelEditorTools.PlaceTrees(terrain, waterPlane, seed, terrainSize, editorResources.maxTreesCount, treesDensity, treesByEdgesOnly);
	}

	private int PlaceExtraObjects()
	{
		return LevelEditorTools.PlaceExtraObjects(terrain, waterPlane, seed, usedExtraObjects, editorResources.extraObjectsDictionary, terrainSize, lastPlacedExtraObjectID);
	}

	private void PlaceCliffs()
	{
		LevelEditorTools.PlaceCliffs(terrain, editorResources.cliffPrefabs, terrainSize, seed, editorResources.baseCliffsCount, editorResources.minHillAngle);
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
		return LevelEditorTools.ApplyAddExtraObjectsStamp(terrain, waterPlane, stamp, editorResources.stampTextures, seed, editorResources.extraObjectsDictionary, terrainSize, waterEnabled, ref addedTrees, ref addedExtraObjects, lastPlacedExtraObjectID);
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
		return LevelEditorTools.ApplyAddExtraObjectsPath(terrain, waterPlane, path, editorResources.pathPatterns, seed, editorResources.extraObjectsDictionary, terrainSize, waterEnabled, ref addedTrees, ref addedExtraObjects, lastPlacedExtraObjectID);
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
