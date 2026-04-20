using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditorUI : MonoBehaviour
{
	private LevelEditor levelEditor;

	private LevelEditorResources editorResources;

	private Camera mainCamera;

	public Button modifyTerrainButton;

	public Button placeObjectsButton;

	public Button finalizeButton;

	public Button placeRoutesButton;

	public Button addMudButton;

	public GameObject messageWindow;

	public Text messageText;

	public Text messageCaption;

	[Header("Step 1 - terrain generation")]
	public GameObject generateTerrainWindow;

	public Slider seedSlider;

	public Slider sizeSlider;

	public Slider bumpsStrengthSlider;

	public Slider treeDensitySlider;

	public Toggle onlyOnEdgesToggle;

	public Toggle flatCenterToggle;

	public GameObject modifyExtraObjectWindow;

	public Dropdown extraObjectDropdown;

	public Slider extraObjectDensitySlider;

	public Toggle extraObjectEdgesOnlyToggle;

	public ExtraObjectButton[] extraObjectButtons;

	public Image[] patternImages;

	public int selectedTerrainPatternID;

	public Toggle enableWaterToggle;

	public Slider waterHeightSlider;

	public Toggle frozenWaterToggle;

	public Image randomGenerationButton;

	public Image advancedGenerationButton;

	public GameObject randomGenerationWindow;

	public GameObject advancedGenerationWindow;

	private int lastExtraObjectSelected;

	private ExtraObjectReference[] usedExtraObjects;

	[Header("Step 2 - terrain modifying")]
	public GameObject modifyTerrainWindow;

	public GameObject tapOnScreenText;

	public GameObject tapTodrawPathText;

	public GameObject drawPathText;

	public Image[] stampButtons;

	public Image[] pathPatternButtons;

	public Outline[] modActionButtons;

	public Image[] paintTextureButtons;

	public Color selectedButtonColor;

	public Color deselectedButtonColor;

	public GameObject placeStampWarning;

	public CanvasGroup modsToolsPanel;

	public Text modificationTypeText;

	public Slider modStrengthSlider;

	public GameObject stampSettingsPanel;

	public GameObject pathSettingsPanel;

	public GameObject terrainPatternPanel;

	public GameObject moveStampButton;

	public GameObject sizeStampButton;

	public GameObject removeStampButton;

	public GameObject removePathButton;

	public GameObject processingTerrainWindow;

	public Text processingTerrainText;

	public GameObject modsResetWarningWindow;

	public Text modStrengthText;

	public Dropdown addingExtraObjectDropdown;

	public GameObject objectToAddWindow;

	public Image stampModTypeButton;

	public Image pathModTypeButton;

	public Color selectedModTypeButtonColor;

	public Color deselectedModTypeButtonColor;

	public GameObject stampSelectorWindow;

	public GameObject pathSelectorWindow;

	[Header("Step 3 - placing props")]
	public GameObject placingPropsWindow;

	public GameObject propSelectorWindow;

	public PropButton propButtonExample;

	public GameObject propButtonsParent;

	public Text selectedPropNameText;

	public GameObject tapToPlacePropText;

	public GameObject selectPropText;

	public GameObject movePropButton;

	public GameObject sizePropButton;

	public GameObject removePropButton;

	public GameObject liftPropButton;

	public Text scaleValueText;

	public CanvasGroup propInteractionMenu;

	public Toggle alignBySlopeToggle;

	public Toggle snapToggle;

	public Image[] propCategoryTabs;

	public Color selectedPropCategoryTabColor;

	public Color deselectedPropCategoryTabColor;

	public Toggle extra0Toggle;

	public Toggle extra1Toggle;

	public Text extra0NameText;

	public Text extra1NameText;

	public RectTransform placementWindowsParent;

	[HideInInspector]
	public CircleDrawer circleDrawer;

	public float circleDrawerWidth = 0.5f;

	public float circleDrawerBaseRadius = 5f;

	public float circleDrawerMinRadius = 2f;

	public int circleDrawerPoints = 30;

	public Material circleDrawerMaterial;

	public List<Vector3> directions = new List<Vector3>();

	private int selectedPropCategory;

	[HideInInspector]
	public float currentCircleDrawerRadius;

	[Header("Step 4 - placing routes")]
	public GameObject placingRoutesWindow;

	public RouteButton exampleRouteButton;

	public GameObject tapToPlaceWaypointText;

	public GameObject routeButtonsParent;

	public Color selectedRouteButtonColor;

	public Color deselectedRouteButtonColor;

	public CanvasGroup routeSettingsWindow;

	public InputField routeNameInputField;

	public GameObject moveCheckpointButton;

	public GameObject removeCheckpointButton;

	public GameObject applyCheckpointButton;

	[Header("Step 5 - adding mud")]
	public GameObject addingMudWindow;

	public CanvasGroup mudStampSettingsGroup;

	public Text mudStampsCountText;

	public Outline[] mudStampPatterns;

	public GameObject moveMudStampButton;

	public GameObject removeMudStampButton;

	public GameObject sizeMudStampButton;

	public GameObject applyMudStampButton;

	public GameObject mudWarningMessage;

	public Slider mudDepthSlider;

	public Slider mudViscositySlider;

	[Header("Step 6 - finalizing map")]
	public GameObject finalizingMapWindow;

	public GameObject uploadWarningWindow;

	public Text spawnPointsCountText;

	public Text propsCountText;

	public Button uploadButton;

	public InputField mapNameField;

	public InputField mapDescriptionField;

	public GameObject spawnPointsWarning;

	public GameObject metaWarning;

	public GameObject mapUploadingWindow;

	public Text mapUploadStatusText;

	public Text mapVisiblityStatusText;

	public GameObject menu;

	private int spawnPointsCount;

	private void Awake()
	{
		usedExtraObjects = new ExtraObjectReference[3];
		for (int i = 0; i < usedExtraObjects.Length; i++)
		{
			usedExtraObjects[i] = new ExtraObjectReference();
			usedExtraObjects[i].arrayID = -1;
		}
		editorResources = LevelEditorTools.editorResources;
		circleDrawer = new GameObject("CircleDrawer").AddComponent<CircleDrawer>();
		circleDrawer.pointsCount = circleDrawerPoints;
		circleDrawer.width = circleDrawerWidth;
		circleDrawer.mat = circleDrawerMaterial;
		circleDrawer.enabled = false;
		currentCircleDrawerRadius = circleDrawerBaseRadius;
		levelEditor = GetComponent<LevelEditor>();
		LevelEditor obj = levelEditor;
		obj.OnCreationStepChanged = (LevelEditor.CreationStepChanged)Delegate.Combine(obj.OnCreationStepChanged, new LevelEditor.CreationStepChanged(OnLevelCreationStepChanged));
		LevelEditor obj2 = levelEditor;
		obj2.OnModActionChanged = (LevelEditor.ModActionChanged)Delegate.Combine(obj2.OnModActionChanged, new LevelEditor.ModActionChanged(OnModActionChanged));
		LevelEditor obj3 = levelEditor;
		obj3.OnStampStateChanged = (LevelEditor.StampStateChanged)Delegate.Combine(obj3.OnStampStateChanged, new LevelEditor.StampStateChanged(OnStampStateChanged));
		LevelEditor obj4 = levelEditor;
		obj4.OnProcessingTerrainStarted = (LevelEditor.ProcessingTerrainStarted)Delegate.Combine(obj4.OnProcessingTerrainStarted, new LevelEditor.ProcessingTerrainStarted(OnProcessingTerrainStarted));
		LevelEditor obj5 = levelEditor;
		obj5.OnProcessingTerrainFinished = (LevelEditor.ProcessingTerrainFinished)Delegate.Combine(obj5.OnProcessingTerrainFinished, new LevelEditor.ProcessingTerrainFinished(OnProcessingTerrainFinished));
		LevelEditor obj6 = levelEditor;
		obj6.OnModsResetWarning = (LevelEditor.ModsResetWarning)Delegate.Combine(obj6.OnModsResetWarning, new LevelEditor.ModsResetWarning(OnModsResetWarning));
		LevelEditor obj7 = levelEditor;
		obj7.OnTerrainValuesRandomized = (LevelEditor.TerrainValuesRandomized)Delegate.Combine(obj7.OnTerrainValuesRandomized, new LevelEditor.TerrainValuesRandomized(OnTerrainValuesRandomized));
		LevelEditor obj8 = levelEditor;
		obj8.OnModTypeChanged = (LevelEditor.ModTypeChanged)Delegate.Combine(obj8.OnModTypeChanged, new LevelEditor.ModTypeChanged(OnModTypeChanged));
		LevelEditor obj9 = levelEditor;
		obj9.OnPathStateChanged = (LevelEditor.PathStateChanged)Delegate.Combine(obj9.OnPathStateChanged, new LevelEditor.PathStateChanged(OnPathStateChanged));
		LevelEditor obj10 = levelEditor;
		obj10.OnSelectedPropChanged = (LevelEditor.SelectedPropChanged)Delegate.Combine(obj10.OnSelectedPropChanged, new LevelEditor.SelectedPropChanged(OnSelectedPropChanged));
		LevelEditor obj11 = levelEditor;
		obj11.OnPropStateChanged = (LevelEditor.PropStateChanged)Delegate.Combine(obj11.OnPropStateChanged, new LevelEditor.PropStateChanged(OnPropStateChanged));
		LevelEditor obj12 = levelEditor;
		obj12.OnPlacedObjectScaleChanged = (LevelEditor.PlacedObjectScaleChanged)Delegate.Combine(obj12.OnPlacedObjectScaleChanged, new LevelEditor.PlacedObjectScaleChanged(OnPlacedObjectScaleChanged));
		LevelEditor obj13 = levelEditor;
		obj13.OnTerrainGenerated = (LevelEditor.TerrainGenerated)Delegate.Combine(obj13.OnTerrainGenerated, new LevelEditor.TerrainGenerated(OnTerrainGenerated));
		LevelEditor obj14 = levelEditor;
		obj14.OnRouteCreationStepChanged = (LevelEditor.RouteCreationStepChanged)Delegate.Combine(obj14.OnRouteCreationStepChanged, new LevelEditor.RouteCreationStepChanged(OnRouteCreationStepChanged));
		LevelEditor obj15 = levelEditor;
		obj15.OnLevelUploadFinished = (LevelEditor.LevelUploadFinished)Delegate.Combine(obj15.OnLevelUploadFinished, new LevelEditor.LevelUploadFinished(LevelUploadFinished));
		LevelEditor obj16 = levelEditor;
		obj16.OnSelectedMudStampChanged = (LevelEditor.SelectedMudStampChanged)Delegate.Combine(obj16.OnSelectedMudStampChanged, new LevelEditor.SelectedMudStampChanged(OnSelectedMudStampChanged));
	}

	private void Start()
	{
		InitializeUI();
		mainCamera = Camera.main;
	}

	private void Update()
	{
		PoseButtons();
		UpdateCircleDrawer();
		Color color = scaleValueText.color;
		color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime);
		scaleValueText.color = color;
	}

	private void LevelUploadFinished(bool failed)
	{
		mapUploadingWindow.SetActive(value: false);
		string caption = (!failed) ? "Success" : "Error";
		string body = (!failed) ? "Your map was successfully uploaded!" : "An error occurred while uploading";
		ShowMessage(caption, body);
	}

	private void ShowMessage(string caption, string body)
	{
		messageWindow.SetActive(value: true);
		messageText.text = body;
		messageCaption.text = caption;
	}

	public void InitializeUI()
	{
		generateTerrainWindow.SetActive(value: false);
		seedSlider.value = levelEditor.seed;
		sizeSlider.value = levelEditor.terrainSize;
		bumpsStrengthSlider.value = levelEditor.bumpsStrength;
		treeDensitySlider.value = levelEditor.treesDensity;
		onlyOnEdgesToggle.isOn = levelEditor.treesByEdgesOnly;
		flatCenterToggle.isOn = levelEditor.flatCenter;
		modifyExtraObjectWindow.SetActive(value: false);
		enableWaterToggle.isOn = levelEditor.waterEnabled;
		waterHeightSlider.value = levelEditor.waterHeight;
		frozenWaterToggle.isOn = levelEditor.frozenWater;
		modifyTerrainWindow.SetActive(value: false);
		tapOnScreenText.gameObject.SetActive(value: false);
		drawPathText.gameObject.SetActive(value: false);
		modStrengthSlider.value = levelEditor.modStrength;
		moveStampButton.SetActive(value: false);
		sizeStampButton.SetActive(value: false);
		removeStampButton.SetActive(value: false);
		removePathButton.SetActive(value: false);
		pathSelectorWindow.SetActive(value: false);
		processingTerrainWindow.SetActive(value: false);
		modsResetWarningWindow.SetActive(value: false);
		propSelectorWindow.SetActive(value: false);
		selectedPropNameText.text = "NONE";
		tapToPlacePropText.SetActive(value: false);
		selectPropText.SetActive(value: false);
		movePropButton.SetActive(value: false);
		sizePropButton.SetActive(value: false);
		removePropButton.SetActive(value: false);
		liftPropButton.SetActive(value: false);
		scaleValueText.gameObject.SetActive(value: false);
		alignBySlopeToggle.isOn = levelEditor.alignBySlope;
		extra0Toggle.gameObject.SetActive(value: false);
		extra1Toggle.gameObject.SetActive(value: false);
		UpdateStampButtons();
		ChangeModAction(0);
		ChangePropCategory(0);
		modifyTerrainButton.interactable = false;
		placeObjectsButton.interactable = false;
		finalizeButton.interactable = false;
		placeRoutesButton.interactable = false;
		addMudButton.interactable = false;
		finalizingMapWindow.SetActive(value: false);
		uploadWarningWindow.SetActive(value: false);
		addingMudWindow.SetActive(value: false);
		moveCheckpointButton.SetActive(value: false);
		removeCheckpointButton.SetActive(value: false);
		applyCheckpointButton.SetActive(value: false);
		moveMudStampButton.SetActive(value: false);
		removeMudStampButton.SetActive(value: false);
		sizeMudStampButton.SetActive(value: false);
		applyMudStampButton.SetActive(value: false);
		mapUploadingWindow.SetActive(value: false);
		menu.SetActive(value: false);
		messageWindow.SetActive(value: false);
		ToggleMapGenerationApproach(random: true);
	}

	private void UpdateCircleDrawer()
	{
		if (!circleDrawer.enabled)
		{
			return;
		}
		Transform transform = null;
		float num = currentCircleDrawerRadius;
		if (levelEditor.levelCreationStep == LevelCreationStep.PlacingObjects)
		{
			transform = levelEditor.PlacedProp.transform;
			float num2 = num;
			Vector3 localScale = levelEditor.PlacedProp.transform.localScale;
			num = num2 * (localScale.x * levelEditor.PlacedProp.circleDrawerSizeMultiplier);
		}
		if (levelEditor.levelCreationStep == LevelCreationStep.PlacingRoutes)
		{
			transform = levelEditor.SelectedCheckpoint.transform;
			num = 20f;
		}
		if (transform == null)
		{
			return;
		}
		circleDrawer.transform.position = transform.position;
		Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + transform.forward * num);
		Vector3 screenPos2 = Camera.main.WorldToScreenPoint(transform.position - transform.forward * num);
		Vector3 screenPos3 = Camera.main.WorldToScreenPoint(transform.position + transform.right * num);
		Vector3 screenPos4 = Camera.main.WorldToScreenPoint(transform.position - transform.right * num);
		directions.Clear();
		if (!PosWithinScreen(screenPos))
		{
			Vector3 a = Camera.main.WorldToScreenPoint(transform.position + Vector3.ProjectOnPlane(transform.forward, Vector3.up));
			directions.Add((a - vector).normalized);
		}
		if (!PosWithinScreen(screenPos2))
		{
			Vector3 a2 = Camera.main.WorldToScreenPoint(transform.position - Vector3.ProjectOnPlane(transform.forward, Vector3.up));
			directions.Add((a2 - vector).normalized);
		}
		if (!PosWithinScreen(screenPos3))
		{
			Vector3 a3 = Camera.main.WorldToScreenPoint(transform.position + Vector3.ProjectOnPlane(transform.right, Vector3.up));
			directions.Add((a3 - vector).normalized);
		}
		if (!PosWithinScreen(screenPos4))
		{
			Vector3 a4 = Camera.main.WorldToScreenPoint(transform.position - Vector3.ProjectOnPlane(transform.right, Vector3.up));
			directions.Add((a4 - vector).normalized);
		}
		if (PosWithinScreen(vector))
		{
			foreach (Vector3 direction in directions)
			{
				Vector3 vector2 = ProjectVectorOnScreenBorders(vector, direction);
				Ray ray = Camera.main.ScreenPointToRay(vector2);
				if (new Plane(transform.position, transform.position + Vector3.forward, transform.position + Vector3.right).Raycast(ray, out float enter) && vector2 != Vector3.zero)
				{
					Vector3 point = ray.GetPoint(enter);
					UnityEngine.Debug.DrawRay(point, Vector3.up * 5f, Color.red);
					float num3 = Vector3.Distance(point, transform.position);
					if (num > num3)
					{
						num = num3;
					}
				}
			}
			if (num < circleDrawerMinRadius)
			{
				num = circleDrawerMinRadius;
			}
			circleDrawer.radius = num;
		}
	}

	private bool PosWithinScreen(Vector3 screenPos)
	{
		if (screenPos.x > (float)(Screen.width - 20) || screenPos.x < 20f || screenPos.y > (float)(Screen.height - 20) || screenPos.y < 20f)
		{
			return false;
		}
		return true;
	}

	private Vector3 ProjectVectorOnScreenBorders(Vector3 origin, Vector3 dir)
	{
		origin.z = 0f;
		dir.z = 0f;
		Vector3 intersection;
		if (LineLineIntersection(out intersection, new Vector3(20f, 20f, 0f), new Vector3(Screen.width, 0f, 0f), origin, dir * 10000f) && dir.y < 0f && PosWithinScreen(intersection))
		{
			return intersection;
		}
		if (LineLineIntersection(out intersection, new Vector3(20f, 20f, 0f), new Vector3(0f, Screen.height, 0f), origin, dir * 10000f) && dir.x < 0f && PosWithinScreen(intersection))
		{
			return intersection;
		}
		if (LineLineIntersection(out intersection, new Vector3(20f, Screen.height - 20, 0f), new Vector3(Screen.width, 0f, 0f), origin, dir * 10000f) && dir.y > 0f && PosWithinScreen(intersection))
		{
			return intersection;
		}
		if (LineLineIntersection(out intersection, new Vector3(Screen.width - 20, 20f, 0f), new Vector3(0f, Screen.height, 0f), origin, dir * 10000f) && dir.x > 0f && PosWithinScreen(intersection))
		{
			return intersection;
		}
		return Vector3.zero;
	}

	public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
	{
		Vector3 lhs = linePoint2 - linePoint1;
		Vector3 rhs = Vector3.Cross(lineVec1, lineVec2);
		Vector3 lhs2 = Vector3.Cross(lhs, lineVec2);
		float f = Vector3.Dot(lhs, rhs);
		if (Mathf.Abs(f) < 0.0001f && rhs.sqrMagnitude > 0.0001f)
		{
			float d = Vector3.Dot(lhs2, rhs) / rhs.sqrMagnitude;
			intersection = linePoint1 + lineVec1 * d;
			return true;
		}
		intersection = Vector3.zero;
		return false;
	}

	private float GetMinEdgeDistance(Vector3 _pos)
	{
		float num = 10000f;
		if (_pos.x < num)
		{
			num = _pos.x;
		}
		if ((float)Screen.width - _pos.x < num)
		{
			num = (float)Screen.width - _pos.x;
		}
		if (_pos.y < num)
		{
			num = _pos.y;
		}
		if ((float)Screen.height - _pos.y < num)
		{
			num = (float)Screen.height - _pos.y;
		}
		return num;
	}

	private void PoseButtons()
	{
		if (moveStampButton.activeSelf && !levelEditor.movingStamp)
		{
			Vector3 vector = mainCamera.WorldToScreenPoint(levelEditor.stampPosition);
			if (vector.z > 0f)
			{
				moveStampButton.transform.position = new Vector3(vector.x, vector.y, 0f);
			}
		}
		if (sizeStampButton.activeSelf && !levelEditor.sizingStamp)
		{
			Vector3 vector2 = mainCamera.WorldToScreenPoint(levelEditor.stampPosition + levelEditor.stampProjector.transform.up * levelEditor.stampProjector.orthographicSize);
			if (vector2.z > 0f)
			{
				sizeStampButton.transform.position = new Vector3(vector2.x, vector2.y, 0f);
			}
		}
		if (removeStampButton.activeSelf)
		{
			Vector3 vector3 = mainCamera.WorldToScreenPoint(levelEditor.stampPosition - levelEditor.stampProjector.transform.up * levelEditor.stampProjector.orthographicSize);
			if (vector3.z > 0f)
			{
				removeStampButton.transform.position = new Vector3(vector3.x, vector3.y, 0f);
			}
		}
		if (removePathButton.activeSelf)
		{
			Vector3 vector4 = mainCamera.WorldToScreenPoint(levelEditor.lastPathPoint);
			if (vector4.z > 0f)
			{
				removePathButton.transform.position = new Vector3(vector4.x, vector4.y, 0f);
			}
		}
		if (movePropButton.activeSelf && !levelEditor.movingProp)
		{
			Vector3 vector5 = mainCamera.WorldToScreenPoint(levelEditor.PlacedProp.transform.position + Vector3.ProjectOnPlane(levelEditor.PlacedProp.transform.right, Vector3.up).normalized * circleDrawer.radius);
			if (vector5.z > 0f)
			{
				movePropButton.transform.position = new Vector3(vector5.x, vector5.y, 0f);
			}
		}
		if (sizePropButton.activeSelf && !levelEditor.sizingProp)
		{
			Vector3 vector6 = mainCamera.WorldToScreenPoint(levelEditor.PlacedProp.transform.position + Vector3.ProjectOnPlane(levelEditor.PlacedProp.transform.forward, Vector3.up).normalized * circleDrawer.radius);
			if (vector6.z > 0f)
			{
				sizePropButton.transform.position = new Vector3(vector6.x, vector6.y, 0f);
			}
		}
		if (removePropButton.activeSelf)
		{
			Vector3 vector7 = mainCamera.WorldToScreenPoint(levelEditor.PlacedProp.transform.position - Vector3.ProjectOnPlane(levelEditor.PlacedProp.transform.forward, Vector3.up).normalized * circleDrawer.radius);
			if (vector7.z > 0f)
			{
				removePropButton.transform.position = new Vector3(vector7.x, vector7.y, 0f);
			}
		}
		if (liftPropButton.activeSelf && !levelEditor.liftingProp)
		{
			Vector3 vector8 = mainCamera.WorldToScreenPoint(levelEditor.PlacedProp.transform.position - Vector3.ProjectOnPlane(levelEditor.PlacedProp.transform.right, Vector3.up).normalized * circleDrawer.radius);
			if (vector8.z > 0f)
			{
				liftPropButton.transform.position = new Vector3(vector8.x, vector8.y, 0f);
			}
		}
		if (scaleValueText.gameObject.activeSelf)
		{
			Vector3 vector9 = mainCamera.WorldToScreenPoint(levelEditor.PlacedProp.transform.position);
			if (vector9.z > 0f)
			{
				scaleValueText.transform.position = new Vector3(vector9.x, vector9.y, 0f);
			}
		}
		if (moveCheckpointButton.activeSelf && !levelEditor.movingCheckpoint)
		{
			Vector3 vector10 = mainCamera.WorldToScreenPoint(levelEditor.SelectedCheckpoint.transform.position + Vector3.ProjectOnPlane(levelEditor.SelectedCheckpoint.transform.right, Vector3.up).normalized * 20f);
			if (vector10.z > 0f)
			{
				moveCheckpointButton.transform.position = new Vector3(vector10.x, vector10.y, 0f);
			}
		}
		if (removeCheckpointButton.activeSelf)
		{
			Vector3 vector11 = mainCamera.WorldToScreenPoint(levelEditor.SelectedCheckpoint.transform.position - Vector3.ProjectOnPlane(levelEditor.SelectedCheckpoint.transform.right, Vector3.up).normalized * 20f);
			if (vector11.z > 0f)
			{
				removeCheckpointButton.transform.position = new Vector3(vector11.x, vector11.y, 0f);
			}
		}
		if (applyCheckpointButton.activeSelf)
		{
			Vector3 vector12 = mainCamera.WorldToScreenPoint(levelEditor.SelectedCheckpoint.transform.position + Vector3.ProjectOnPlane(levelEditor.SelectedCheckpoint.transform.forward, Vector3.up).normalized * 20f);
			if (vector12.z > 0f)
			{
				applyCheckpointButton.transform.position = new Vector3(vector12.x, vector12.y, 0f);
			}
		}
		if (moveMudStampButton.activeSelf && !levelEditor.movingMudStamp)
		{
			Vector3 vector13 = mainCamera.WorldToScreenPoint(levelEditor.SelectedMudStamp.stampPosition + Vector3.ProjectOnPlane(levelEditor.SelectedMudStamp.stampIndicator.transform.right, Vector3.up).normalized * levelEditor.SelectedMudStamp.stampSize);
			if (vector13.z > 0f)
			{
				moveMudStampButton.transform.position = new Vector3(vector13.x, vector13.y, 0f);
			}
		}
		if (sizeMudStampButton.activeSelf && !levelEditor.sizingMudStamp)
		{
			Vector3 vector14 = mainCamera.WorldToScreenPoint(levelEditor.SelectedMudStamp.stampPosition + Vector3.ProjectOnPlane(levelEditor.SelectedMudStamp.stampIndicator.transform.forward, Vector3.up).normalized * levelEditor.SelectedMudStamp.stampSize);
			if (vector14.z > 0f)
			{
				sizeMudStampButton.transform.position = new Vector3(vector14.x, vector14.y, 0f);
			}
		}
		if (removeMudStampButton.activeSelf)
		{
			Vector3 vector15 = mainCamera.WorldToScreenPoint(levelEditor.SelectedMudStamp.stampPosition + Vector3.ProjectOnPlane(-levelEditor.SelectedMudStamp.stampIndicator.transform.right, Vector3.up).normalized * levelEditor.SelectedMudStamp.stampSize);
			if (vector15.z > 0f)
			{
				removeMudStampButton.transform.position = new Vector3(vector15.x, vector15.y, 0f);
			}
		}
		if (applyMudStampButton.activeSelf)
		{
			Vector3 vector16 = mainCamera.WorldToScreenPoint(levelEditor.SelectedMudStamp.stampPosition + Vector3.ProjectOnPlane(-levelEditor.SelectedMudStamp.stampIndicator.transform.forward, Vector3.up).normalized * levelEditor.SelectedMudStamp.stampSize);
			if (vector16.z > 0f)
			{
				applyMudStampButton.transform.position = new Vector3(vector16.x, vector16.y, 0f);
			}
		}
	}

	public void ChangeLevelGenerationStep(int stepID)
	{
		levelEditor.ChangeLevelCreationStep((LevelCreationStep)stepID);
	}

	public void GenerateRandomSeed()
	{
		int num = UnityEngine.Random.Range(0, 10000);
		seedSlider.value = num;
	}

	public void SetPattern(int id)
	{
		selectedTerrainPatternID = id;
		UpdatePatternButtons();
	}

	public void SetPaintTextureID(int id)
	{
		levelEditor.ChangeModPaintTextureID(id);
		UpdatePaintTextureButtons();
	}

	public void ChangeStampStrengthDirectly(float value)
	{
		levelEditor.modStrength = value;
	}

	public void ChangeStampID(int ID)
	{
		levelEditor.stampTextureID = ID;
		levelEditor.UpdateStampProjector();
		UpdateStampButtons();
	}

	public void ChangePathPatternID(int ID)
	{
		levelEditor.selectedPathPattern = ID;
		levelEditor.UpdatePathSettings();
		UpdatePathPatternButtons();
	}

	public void ChangePathWidth(float width)
	{
		levelEditor.pathWidth = width;
		levelEditor.UpdatePathSettings();
	}

	private void UpdateStampButtons()
	{
		for (int i = 0; i < stampButtons.Length; i++)
		{
			stampButtons[i].color = ((i != levelEditor.stampTextureID) ? deselectedButtonColor : selectedButtonColor);
		}
	}

	private void UpdatePathPatternButtons()
	{
		for (int i = 0; i < pathPatternButtons.Length; i++)
		{
			pathPatternButtons[i].color = ((i != levelEditor.selectedPathPattern) ? deselectedButtonColor : selectedButtonColor);
		}
	}

	public void ChangeModAction(int actionID)
	{
		levelEditor.ChangeModAction((ModAction)actionID);
	}

	public void ChangeModType(int typeID)
	{
		levelEditor.ChangeModType((TerrainModifyingType)typeID);
	}

	public void RemoveMod()
	{
		levelEditor.RemoveStamp();
	}

	public void PreviewMod()
	{
		levelEditor.addingExtraObjectID = addingExtraObjectDropdown.value - 1;
		if (levelEditor.terrainModType == TerrainModifyingType.Stamp)
		{
			levelEditor.PreviewStamp();
		}
		if (levelEditor.terrainModType == TerrainModifyingType.Path)
		{
			levelEditor.PreviewPath();
		}
	}

	public void ApplyMod()
	{
		levelEditor.addingExtraObjectID = addingExtraObjectDropdown.value - 1;
		if (levelEditor.terrainModType == TerrainModifyingType.Stamp)
		{
			levelEditor.ApplyAndSaveStamp();
		}
		if (levelEditor.terrainModType == TerrainModifyingType.Path)
		{
			levelEditor.ApplyAndSavePath();
		}
	}

	public void GenerateTerrain()
	{
		levelEditor.seed = (int)seedSlider.value;
		levelEditor.terrainSize = sizeSlider.value;
		levelEditor.bumpsStrength = bumpsStrengthSlider.value;
		levelEditor.flatCenter = flatCenterToggle.isOn;
		levelEditor.treesDensity = treeDensitySlider.value;
		levelEditor.treesByEdgesOnly = onlyOnEdgesToggle.isOn;
		levelEditor.mainTextureID = selectedTerrainPatternID;
		levelEditor.waterEnabled = enableWaterToggle.isOn;
		levelEditor.waterHeight = waterHeightSlider.value;
		levelEditor.frozenWater = frozenWaterToggle.isOn;
		levelEditor.usedExtraObjects[0] = usedExtraObjects[0].DeepCopy();
		levelEditor.usedExtraObjects[1] = usedExtraObjects[1].DeepCopy();
		levelEditor.usedExtraObjects[2] = usedExtraObjects[2].DeepCopy();
		levelEditor.GenerateTerrain(dontShowNotification: false);
	}

	public void GenerateRandomTerrain()
	{
		levelEditor.GenerateRandomTerrain();
	}

	public void SelectProp(int propID)
	{
		levelEditor.SelectProp(propID);
		propSelectorWindow.SetActive(value: false);
	}

	public void OpenPropSelector()
	{
		BuildPropsGrid(selectedPropCategory);
		propSelectorWindow.SetActive(value: true);
	}

	public void ApplyProp()
	{
		levelEditor.ApplyProp();
	}

	public void DuplicateProp()
	{
		levelEditor.DuplicateProp();
	}

	public void ResetPropScale()
	{
		levelEditor.ResetPropScale();
	}

	public void AlignBySlope(bool align)
	{
		levelEditor.SetAlignBySlope(align);
	}

	public void SetSnapping(bool snap)
	{
	}

	public void RemoveProp()
	{
		levelEditor.RemoveProp();
	}

	public void ChangePropCategory(int newCategory)
	{
		selectedPropCategory = newCategory;
		BuildPropsGrid(selectedPropCategory);
		for (int i = 0; i < propCategoryTabs.Length; i++)
		{
			propCategoryTabs[i].color = ((i != newCategory) ? deselectedPropCategoryTabColor : selectedPropCategoryTabColor);
		}
	}

	public void ToggleExtra0(bool on)
	{
		if (levelEditor.PlacedProp != null)
		{
			levelEditor.PlacedProp.ToggleExtra0(on);
		}
	}

	public void ToggleExtra1(bool on)
	{
		if (levelEditor.PlacedProp != null)
		{
			levelEditor.PlacedProp.ToggleExtra1(on);
		}
	}

	public void ShowUploadWarning()
	{
		uploadWarningWindow.SetActive(value: true);
	}

	public void UploadMap(bool makeVisible)
	{
		mapUploadingWindow.SetActive(value: true);
		levelEditor.mapVisible = makeVisible;
		levelEditor.UploadMap();
	}

	public void MapMetaChanged()
	{
		levelEditor.mapName = mapNameField.text;
		levelEditor.mapDescription = mapDescriptionField.text;
		uploadButton.interactable = (spawnPointsCount >= 8 && mapNameField.text != string.Empty && mapDescriptionField.text != string.Empty);
		metaWarning.SetActive(mapNameField.text == string.Empty || mapDescriptionField.text == string.Empty);
	}

	public void CreateNewRoute()
	{
		levelEditor.CreateNewRoute();
	}

	public void SelectRoute(int id)
	{
		levelEditor.SelectRoute(id);
	}

	private void UpdateRouteList()
	{
		RouteButton[] componentsInChildren = routeButtonsParent.GetComponentsInChildren<RouteButton>(includeInactive: false);
		foreach (RouteButton routeButton in componentsInChildren)
		{
			UnityEngine.Object.Destroy(routeButton.gameObject);
		}
		for (int j = 0; j < levelEditor.routes.Count; j++)
		{
			RouteButton component = UnityEngine.Object.Instantiate(exampleRouteButton, exampleRouteButton.transform.parent).GetComponent<RouteButton>();
			component.routeNameText.text = levelEditor.routes[j].routeName;
			component.GetComponent<Image>().color = ((j != levelEditor.selectedRouteID) ? deselectedRouteButtonColor : selectedRouteButtonColor);
			int _id = j;
			component.GetComponent<Button>().onClick.AddListener(delegate
			{
				SelectRoute(_id);
			});
			component.gameObject.SetActive(value: true);
			component.transform.SetSiblingIndex(2);
		}
	}

	public void RouteNameChanged(string routeName)
	{
		levelEditor.ChangeCurrentRouteName(routeName);
		UpdateRouteList();
	}

	public void RemoveCurrentRoute()
	{
		levelEditor.RemoveCurrentRoute();
		UpdateRouteList();
	}

	public void ApplyRoute()
	{
		levelEditor.ApplyRoute();
	}

	public void ShowMenu()
	{
		menu.SetActive(value: true);
	}

	public void LeaveToMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void ToggleMapGenerationApproach(bool random)
	{
		randomGenerationButton.color = ((!random) ? deselectedButtonColor : selectedButtonColor);
		advancedGenerationButton.color = ((!random) ? selectedButtonColor : deselectedButtonColor);
		randomGenerationWindow.SetActive(random);
		advancedGenerationWindow.SetActive(!random);
	}

	public void AddSpawnPoint()
	{
		levelEditor.ChangeLevelCreationStep(LevelCreationStep.PlacingObjects);
		SelectProp(levelEditor.spawnPointPropID);
	}

	private void UpdateMudStampPatternButtons()
	{
		int num = -1;
		if (levelEditor.SelectedMudStamp != null)
		{
			num = levelEditor.SelectedMudStamp.stampTextureID;
		}
		for (int i = 0; i < mudStampPatterns.Length; i++)
		{
			mudStampPatterns[i].effectColor = ((i != num) ? deselectedModTypeButtonColor : selectedModTypeButtonColor);
		}
	}

	public void ChangeMudStampPattern(int ID)
	{
		levelEditor.ChangeMudStampPattern(ID);
		UpdateMudStampPatternButtons();
	}

	public void ChangeMudDepth(float depth)
	{
		levelEditor.ChangeMudDepth(depth);
	}

	public void ChangeMudViscosity(float viscosity)
	{
		levelEditor.ChangeMudViscosity(viscosity);
	}

	public void RemoveAllMudStamps()
	{
		levelEditor.RemoveAllMudStamps();
	}

	private void OnModActionChanged()
	{
		stampSettingsPanel.SetActive(value: true);
		terrainPatternPanel.SetActive(value: false);
		objectToAddWindow.SetActive(value: false);
		switch (levelEditor.modAction)
		{
		case ModAction.LandscapeLowering:
			modificationTypeText.text = "Lowering landscape";
			modStrengthText.text = "Height";
			break;
		case ModAction.LandscapeRaising:
			modificationTypeText.text = "Raising landscape";
			modStrengthText.text = "Height";
			break;
		case ModAction.Smoothing:
			modificationTypeText.text = "Flattening landscape";
			modStrengthText.text = "Flattening power";
			break;
		case ModAction.Painting:
			modificationTypeText.text = "Painting pattern";
			modStrengthText.text = "Paint power";
			terrainPatternPanel.SetActive(value: true);
			break;
		case ModAction.AddingExtraObjects:
			modificationTypeText.text = "Adding extra objects";
			modStrengthText.text = "Density";
			UpdateAddingExtraObjectDropdown();
			objectToAddWindow.SetActive(value: true);
			break;
		case ModAction.RemovingExtraObjects:
			modificationTypeText.text = "Cutting extra objects";
			modStrengthText.text = "Cut power";
			break;
		}
		UpdateStampActionButtons();
	}

	private void OnModTypeChanged()
	{
		tapOnScreenText.SetActive(levelEditor.stampState == StampState.NotPlaced && levelEditor.terrainModType == TerrainModifyingType.Stamp);
		tapTodrawPathText.SetActive(levelEditor.pathState == PathState.NotDrawn && levelEditor.terrainModType == TerrainModifyingType.Path);
		drawPathText.SetActive(levelEditor.pathState == PathState.Drawing && levelEditor.terrainModType == TerrainModifyingType.Path);
		UpdateModTypeButtons();
		pathSettingsPanel.SetActive(levelEditor.terrainModType == TerrainModifyingType.Path);
		stampSelectorWindow.SetActive(levelEditor.terrainModType == TerrainModifyingType.Stamp);
		pathSelectorWindow.SetActive(levelEditor.terrainModType == TerrainModifyingType.Path);
	}

	private void OnPathStateChanged()
	{
		tapTodrawPathText.SetActive(levelEditor.pathState == PathState.NotDrawn);
		drawPathText.SetActive(levelEditor.pathState == PathState.Drawing);
		modsToolsPanel.interactable = (levelEditor.pathState == PathState.FinishedDrawing);
		removePathButton.SetActive(levelEditor.pathState == PathState.FinishedDrawing);
		placeStampWarning.SetActive(levelEditor.pathState == PathState.NotDrawn);
	}

	private void OnLevelCreationStepChanged()
	{
		moveStampButton.SetActive(value: false);
		sizeStampButton.SetActive(value: false);
		removeStampButton.SetActive(value: false);
		removePathButton.SetActive(value: false);
		movePropButton.SetActive(value: false);
		sizePropButton.SetActive(value: false);
		removePropButton.SetActive(value: false);
		liftPropButton.SetActive(value: false);
		scaleValueText.gameObject.SetActive(value: false);
		propSelectorWindow.SetActive(value: false);
		modsResetWarningWindow.SetActive(value: false);
		LevelCreationStep levelCreationStep = levelEditor.levelCreationStep;
		generateTerrainWindow.SetActive(levelCreationStep == LevelCreationStep.Generation);
		modifyExtraObjectWindow.SetActive(value: false);
		placingPropsWindow.SetActive(levelCreationStep == LevelCreationStep.PlacingObjects);
		modifyTerrainWindow.SetActive(levelCreationStep == LevelCreationStep.Modifying);
		UpdateStampButtons();
		UpdateExtraObjectsButtons();
		tapOnScreenText.SetActive(levelCreationStep == LevelCreationStep.Modifying && levelEditor.stampState == StampState.NotPlaced && levelEditor.terrainModType == TerrainModifyingType.Stamp);
		tapTodrawPathText.SetActive(levelCreationStep == LevelCreationStep.Modifying && levelEditor.pathState == PathState.NotDrawn && levelEditor.terrainModType == TerrainModifyingType.Path);
		drawPathText.SetActive(levelCreationStep == LevelCreationStep.Modifying && levelEditor.pathState == PathState.Drawing && levelEditor.terrainModType == TerrainModifyingType.Path);
		tapToPlacePropText.SetActive(value: false);
		selectPropText.SetActive(value: true);
		finalizingMapWindow.SetActive(levelCreationStep == LevelCreationStep.Finalizing);
		addingMudWindow.SetActive(levelCreationStep == LevelCreationStep.AddingMud);
		UpdatePatternButtons();
		UpdatePaintTextureButtons();
		UpdateStampActionButtons();
		UpdateModTypeButtons();
		string mapName = levelEditor.mapName;
		string mapDescription = levelEditor.mapDescription;
		mapDescriptionField.text = mapDescription;
		mapNameField.text = mapName;
		uploadWarningWindow.SetActive(value: false);
		if (levelCreationStep == LevelCreationStep.Finalizing)
		{
			spawnPointsCount = levelEditor.GetSpawnPoints().Length;
			int num = levelEditor.CountProps();
			propsCountText.text = num.ToString();
			spawnPointsCountText.text = spawnPointsCount + "/8";
			uploadButton.interactable = (spawnPointsCount >= 8 && mapNameField.text != string.Empty && mapDescriptionField.text != string.Empty);
			metaWarning.SetActive(mapNameField.text == string.Empty || mapDescriptionField.text == string.Empty);
			spawnPointsWarning.SetActive(spawnPointsCount < 8);
		}
		placingRoutesWindow.SetActive(levelCreationStep == LevelCreationStep.PlacingRoutes);
		tapToPlaceWaypointText.SetActive(value: false);
		moveCheckpointButton.SetActive(value: false);
		removeCheckpointButton.SetActive(value: false);
		applyCheckpointButton.SetActive(value: false);
		mapUploadStatusText.text = ((!levelEditor.mapUploaded) ? "Not uploaded" : "Uploaded");
		mapUploadStatusText.color = ((!levelEditor.mapUploaded) ? Color.red : Color.green);
		mapVisiblityStatusText.text = ((!levelEditor.mapVisible) ? "Hidden" : "Visible");
		mapVisiblityStatusText.color = ((!levelEditor.mapVisible) ? Color.blue : Color.green);
		if (!levelEditor.mapUploaded)
		{
			mapVisiblityStatusText.text = "Not uploaded";
			mapVisiblityStatusText.color = Color.red;
		}
	}

	private void OnStampStateChanged()
	{
		modsToolsPanel.interactable = false;
		placeStampWarning.SetActive(value: true);
		sizeStampButton.SetActive(value: false);
		moveStampButton.SetActive(value: false);
		removeStampButton.SetActive(value: false);
		switch (levelEditor.stampState)
		{
		case StampState.NotPlaced:
			if (levelEditor.levelCreationStep == LevelCreationStep.Modifying)
			{
				tapOnScreenText.gameObject.SetActive(value: true);
			}
			break;
		case StampState.Placed:
			modsToolsPanel.interactable = true;
			placeStampWarning.SetActive(value: false);
			tapOnScreenText.gameObject.SetActive(value: false);
			sizeStampButton.SetActive(value: true);
			moveStampButton.SetActive(value: true);
			removeStampButton.SetActive(value: true);
			break;
		}
	}

	private void OnModsResetWarning()
	{
		modsResetWarningWindow.SetActive(value: true);
	}

	public void OnProcessingTerrainStarted(string text)
	{
		processingTerrainWindow.SetActive(value: true);
		processingTerrainText.text = text;
	}

	public void OnProcessingTerrainFinished()
	{
		processingTerrainWindow.SetActive(value: false);
	}

	private void OnTerrainValuesRandomized()
	{
		seedSlider.value = levelEditor.seed;
		sizeSlider.value = levelEditor.terrainSize;
		bumpsStrengthSlider.value = levelEditor.bumpsStrength;
		treeDensitySlider.value = levelEditor.treesDensity;
		onlyOnEdgesToggle.isOn = levelEditor.treesByEdgesOnly;
		flatCenterToggle.isOn = levelEditor.flatCenter;
		enableWaterToggle.isOn = levelEditor.waterEnabled;
		waterHeightSlider.value = levelEditor.waterHeight;
		usedExtraObjects[0] = levelEditor.usedExtraObjects[0].DeepCopy();
		usedExtraObjects[1] = levelEditor.usedExtraObjects[1].DeepCopy();
		usedExtraObjects[2] = levelEditor.usedExtraObjects[2].DeepCopy();
		UpdateExtraObjectsButtons();
	}

	private void OnSelectedPropChanged()
	{
		if (levelEditor.selectedPropID == -1)
		{
			selectedPropNameText.text = "NONE";
			return;
		}
		selectedPropNameText.text = editorResources.propsDictionary[levelEditor.selectedPropID].propName;
		extra0NameText.text = editorResources.propsDictionary[levelEditor.selectedPropID].extra0Name;
		extra1NameText.text = editorResources.propsDictionary[levelEditor.selectedPropID].extra1Name;
	}

	private void OnPropStateChanged()
	{
		tapToPlacePropText.SetActive(levelEditor.propState == PropPlacementState.Selected);
		selectPropText.SetActive(levelEditor.propState == PropPlacementState.NotSelected);
		movePropButton.SetActive(levelEditor.propState == PropPlacementState.Placed);
		sizePropButton.SetActive(levelEditor.propState == PropPlacementState.Placed);
		removePropButton.SetActive(levelEditor.propState == PropPlacementState.Placed);
		liftPropButton.SetActive(levelEditor.propState == PropPlacementState.Placed);
		scaleValueText.gameObject.SetActive(levelEditor.propState == PropPlacementState.Placed);
		circleDrawer.enabled = (levelEditor.propState == PropPlacementState.Placed);
		propInteractionMenu.interactable = (levelEditor.propState == PropPlacementState.Placed);
		if (levelEditor.propState == PropPlacementState.Placed)
		{
			if (levelEditor.PlacedProp != null)
			{
				extra0Toggle.isOn = levelEditor.PlacedProp.extra0Enabled;
				extra1Toggle.isOn = levelEditor.PlacedProp.extra1Enabled;
				extra0Toggle.gameObject.SetActive(levelEditor.PlacedProp.extra0 != null);
				extra1Toggle.gameObject.SetActive(levelEditor.PlacedProp.extra1 != null);
			}
		}
		else
		{
			extra0Toggle.gameObject.SetActive(value: false);
			extra1Toggle.gameObject.SetActive(value: false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(placementWindowsParent);
	}

	private void OnPlacedObjectScaleChanged(float newScale)
	{
		scaleValueText.text = "Size: " + Mathf.Round(newScale * 10f) / 10f;
		scaleValueText.color = Color.white;
	}

	private void OnTerrainGenerated()
	{
		modifyTerrainButton.interactable = true;
		placeObjectsButton.interactable = true;
		finalizeButton.interactable = true;
		placeRoutesButton.interactable = true;
		addMudButton.interactable = true;
	}

	private void OnRouteCreationStepChanged()
	{
		UpdateRouteList();
		tapToPlaceWaypointText.SetActive(levelEditor.routeCreationStep == RouteCreationStep.Selected);
		routeSettingsWindow.interactable = (levelEditor.routeCreationStep == RouteCreationStep.Selected);
		if (levelEditor.selectedRouteID != -1)
		{
			routeNameInputField.text = levelEditor.routes[levelEditor.selectedRouteID].routeName;
		}
		moveCheckpointButton.SetActive(levelEditor.routeCreationStep == RouteCreationStep.ModifyingCheckpoint);
		removeCheckpointButton.SetActive(levelEditor.routeCreationStep == RouteCreationStep.ModifyingCheckpoint);
		applyCheckpointButton.SetActive(levelEditor.routeCreationStep == RouteCreationStep.ModifyingCheckpoint);
		circleDrawer.enabled = (levelEditor.routeCreationStep == RouteCreationStep.ModifyingCheckpoint);
	}

	private void OnSelectedMudStampChanged()
	{
		moveMudStampButton.SetActive(levelEditor.mudStampState == MudStampState.Selected);
		removeMudStampButton.SetActive(levelEditor.mudStampState == MudStampState.Selected);
		sizeMudStampButton.SetActive(levelEditor.mudStampState == MudStampState.Selected);
		applyMudStampButton.SetActive(levelEditor.mudStampState == MudStampState.Selected);
		mudStampSettingsGroup.interactable = (levelEditor.mudStampState == MudStampState.Selected);
		mudWarningMessage.SetActive(levelEditor.mudStampState == MudStampState.NotSelected);
		if (levelEditor.mudStampState == MudStampState.Selected)
		{
			mudDepthSlider.value = levelEditor.SelectedMudStamp.mudDepth;
			mudViscositySlider.value = levelEditor.SelectedMudStamp.mudViscosity;
		}
		mudStampsCountText.text = levelEditor.mudStamps.Count + "/3";
		UpdateMudStampPatternButtons();
	}

	public void ModifyExtraObject(int objID)
	{
		extraObjectDensitySlider.value = usedExtraObjects[objID].density;
		extraObjectEdgesOnlyToggle.isOn = usedExtraObjects[objID].onlyByEdges;
		extraObjectDropdown.value = usedExtraObjects[objID].arrayID + 1;
		extraObjectDropdown.ClearOptions();
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		for (int i = -1; i < editorResources.extraObjectsDictionary.Length; i++)
		{
			Dropdown.OptionData optionData = new Dropdown.OptionData();
			if (i == -1)
			{
				optionData.text = "NONE";
			}
			else
			{
				optionData.text = editorResources.extraObjectsDictionary[i].displayedName;
			}
			list.Add(optionData);
		}
		extraObjectDropdown.options = list;
		modifyExtraObjectWindow.SetActive(value: true);
		lastExtraObjectSelected = objID;
	}

	private void UpdateAddingExtraObjectDropdown()
	{
		addingExtraObjectDropdown.ClearOptions();
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		for (int i = -1; i < editorResources.extraObjectsDictionary.Length; i++)
		{
			Dropdown.OptionData optionData = new Dropdown.OptionData();
			if (i == -1)
			{
				optionData.text = "Trees";
			}
			else
			{
				optionData.text = editorResources.extraObjectsDictionary[i].displayedName;
			}
			list.Add(optionData);
		}
		addingExtraObjectDropdown.options = list;
	}

	[ContextMenu("Update extra obj buttons")]
	private void UpdateExtraObjectsButtons()
	{
		for (int i = 0; i < usedExtraObjects.Length; i++)
		{
			ExtraObjectArray extraObjectArray = null;
			if (usedExtraObjects[i].arrayID > -1)
			{
				extraObjectArray = editorResources.extraObjectsDictionary[usedExtraObjects[i].arrayID];
			}
			string text = "NONE";
			if (extraObjectArray != null)
			{
				text = extraObjectArray.displayedName;
				text += "\n";
				text = text + "Density: " + (Mathf.Round(usedExtraObjects[i].density * 10f) / 10f).ToString();
				if (usedExtraObjects[i].onlyByEdges)
				{
					text += ". By edges";
				}
			}
			extraObjectButtons[i].extraObjectNameText.text = text;
		}
	}

	private void UpdateModTypeButtons()
	{
		stampModTypeButton.color = ((levelEditor.terrainModType != 0) ? deselectedModTypeButtonColor : selectedModTypeButtonColor);
		pathModTypeButton.color = ((levelEditor.terrainModType != TerrainModifyingType.Path) ? deselectedModTypeButtonColor : selectedModTypeButtonColor);
	}

	private void UpdatePatternButtons()
	{
		for (int i = 0; i < patternImages.Length; i++)
		{
			patternImages[i].color = ((i != selectedTerrainPatternID) ? deselectedButtonColor : selectedButtonColor);
		}
	}

	private void UpdateStampActionButtons()
	{
		for (int i = 0; i < modActionButtons.Length; i++)
		{
			modActionButtons[i].effectColor = ((i != (int)levelEditor.modAction) ? deselectedButtonColor : selectedButtonColor);
		}
	}

	private void UpdatePaintTextureButtons()
	{
		for (int i = 0; i < paintTextureButtons.Length; i++)
		{
			paintTextureButtons[i].color = ((i != levelEditor.modPaintTextureID) ? deselectedButtonColor : selectedButtonColor);
		}
	}

	private void BuildPropsGrid(int propType)
	{
		propButtonExample.gameObject.SetActive(value: false);
		PropButton[] componentsInChildren = propButtonsParent.GetComponentsInChildren<PropButton>(includeInactive: false);
		foreach (PropButton propButton in componentsInChildren)
		{
			UnityEngine.Object.Destroy(propButton.gameObject);
		}
		for (int j = 0; j < editorResources.propsDictionary.Length; j++)
		{
			if (editorResources.propsDictionary[j].propType == (PropType)propType)
			{
				PropButton component = UnityEngine.Object.Instantiate(propButtonExample.gameObject, propButtonsParent.transform).GetComponent<PropButton>();
				component.propNameText.text = editorResources.propsDictionary[j].propName;
				component.propImage.sprite = editorResources.propsDictionary[j].propImage;
				component.propID = j;
				int propID = j;
				component.GetComponent<Button>().onClick.AddListener(delegate
				{
					SelectProp(propID);
				});
				component.gameObject.SetActive(value: true);
			}
		}
	}

	public void SaveExtraObjectChanges()
	{
		usedExtraObjects[lastExtraObjectSelected].density = extraObjectDensitySlider.value;
		usedExtraObjects[lastExtraObjectSelected].onlyByEdges = extraObjectEdgesOnlyToggle.isOn;
		usedExtraObjects[lastExtraObjectSelected].arrayID = extraObjectDropdown.value - 1;
		modifyExtraObjectWindow.SetActive(value: false);
		UpdateExtraObjectsButtons();
	}
}
