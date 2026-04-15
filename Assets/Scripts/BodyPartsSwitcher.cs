using Battlehub.Integration;
using Battlehub.MeshTools;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsSwitcher : MonoBehaviour
{
	[HideInInspector]
	public Renderer[] AllRenderers;

	public Color BodyColor;

	public bool GlossyPaint;

	public bool GlossyPaintPurchased;

	public List<Wrap> WrapLayers = new List<Wrap>();

	public Color WrapColor;

	public Vector4 WrapCoords;

	public int AppliedWrapID;

	public int WrapLayerCount;

	public int SessionWrapLayerCount;

	[HideInInspector]
	public int CurrentWrapID;

	[HideInInspector]
	public Color CurrentWrapColor;

	[HideInInspector]
	public Vector4 CurrentWrapCoords = new Vector4(0f, 0f, 1f, 1f);

	public Color FRimsColor;

	public Color FBeadlocksColor;

	public Color RRimsColor;

	public Color RBeadlocksColor;

	[HideInInspector]
	public float MudWetness;

	[HideInInspector]
	public float Dirtiness;

	[Space(10f)]
	public GameObject BaseMesh;

	public GameObject Winch;

	public Transform FrontWinchPoint;

	public Transform RearWinchPoint;

	public GameObject RepairPack;

	[SerializeField]
	public PartGroup[] partGroups;

	[SerializeField]
	public DynamicPositionPart[] dynamicParts;

	[SerializeField]
	public TriggerPartGroup[] triggerPartGroups;

	[SerializeField]
	public GameObject StockEngine;

	[SerializeField]
	public GameObject BlowerEngine;

	[SerializeField]
	public GameObject TurboEngine;

	private bool MeshesMerged;

	private GameObject ResultMesh;

	private MenuManager menuManager;

	[HideInInspector]
	public bool Washing;

	private float WashingSpeed = 1f;

	[HideInInspector]
	public bool WinchInstalled;

	[HideInInspector]
	public bool RepairPackInstalled;

	private PhotonView photonView;

	private SuspensionController suspensionController;

	private CarUIControl carUIControl;

	private Texture2D BakedWrap;

	private bool IsWrapBaked;

	private MaterialPropertyBlock propBlock;

	private Texture2D FrontRimTexture;

	private Texture2D RearRimTexture;

	public void GenerateRimsTexture()
	{
		if (FRimsColor == Color.clear)
		{
			FRimsColor = Color.grey;
		}
		if (RRimsColor == Color.clear)
		{
			RRimsColor = Color.grey;
		}
		if (FBeadlocksColor == Color.clear)
		{
			FBeadlocksColor = new Color(0.1f, 0.1f, 0.1f, 1f);
		}
		if (RBeadlocksColor == Color.clear)
		{
			RBeadlocksColor = new Color(0.1f, 0.1f, 0.1f, 1f);
		}
		FrontRimTexture = new Texture2D(2, 2);
		RearRimTexture = new Texture2D(2, 2);
		FrontRimTexture.filterMode = FilterMode.Point;
		FrontRimTexture.SetPixel(1, 1, FBeadlocksColor);
		FrontRimTexture.SetPixel(0, 1, FRimsColor);
		FrontRimTexture.SetPixel(1, 0, Color.white);
		FrontRimTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1f));
		FrontRimTexture.Apply();
		RearRimTexture.filterMode = FilterMode.Point;
		RearRimTexture.SetPixel(1, 1, RBeadlocksColor);
		RearRimTexture.SetPixel(0, 1, RRimsColor);
		RearRimTexture.SetPixel(1, 0, Color.white);
		RearRimTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1f));
		RearRimTexture.Apply();
		propBlock = new MaterialPropertyBlock();
		if (!suspensionController.FrontWheelsControls.TankTracks)
		{
			GameObject[] frontRims = suspensionController.FrontRims;
			foreach (GameObject gameObject in frontRims)
			{
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.GetPropertyBlock(propBlock);
					propBlock.SetTexture("_Texture", FrontRimTexture);
					renderer.SetPropertyBlock(propBlock);
				}
			}
		}
		if (suspensionController.RearWheelsControls.TankTracks)
		{
			return;
		}
		GameObject[] rearRims = suspensionController.RearRims;
		foreach (GameObject gameObject2 in rearRims)
		{
			Renderer[] componentsInChildren2 = gameObject2.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer2 in componentsInChildren2)
			{
				renderer2.GetPropertyBlock(propBlock);
				propBlock.SetTexture("_Texture", RearRimTexture);
				renderer2.SetPropertyBlock(propBlock);
			}
		}
	}

	public void UpdateEngineModel(EngineType engineType)
	{
		UnityEngine.Debug.Log("Updating engine model: " + engineType.ToString());
		if (StockEngine != null)
		{
			StockEngine.SetActive(engineType == EngineType.Stock);
		}
		if (TurboEngine != null)
		{
			TurboEngine.SetActive(engineType == EngineType.Turbo);
		}
		if (BlowerEngine != null)
		{
			BlowerEngine.SetActive(engineType == EngineType.Blower);
		}
	}

	public void InstallBodyPart(PartGroup group, int partID)
	{
		for (int i = 0; i < group.Parts.Length; i++)
		{
			if (group.Parts[i] != null)
			{
				group.Parts[i].SetActive(i == partID);
			}
		}
		group.InstalledPart = partID;
		CheckDynamicParts();
		group.PaintPart();
		CheckTriggerPartGroups();
	}

	public void CheckTriggerPartGroups()
	{
		if (triggerPartGroups == null)
		{
			return;
		}
		TriggerPartGroup[] array = triggerPartGroups;
		foreach (TriggerPartGroup triggerPartGroup in array)
		{
			bool flag = false;
			GameObject[] triggerParts = triggerPartGroup.TriggerParts;
			foreach (GameObject gameObject in triggerParts)
			{
				if (gameObject.activeSelf)
				{
					flag = true;
				}
			}
			GameObject[] partsToToggle = triggerPartGroup.PartsToToggle;
			foreach (GameObject gameObject2 in partsToToggle)
			{
				gameObject2.SetActive(!flag);
			}
		}
	}

	public void CheckDynamicParts()
	{
		if (dynamicParts != null)
		{
			DynamicPositionPart[] array = dynamicParts;
			foreach (DynamicPositionPart dynamicPositionPart in array)
			{
				dynamicPositionPart.UpdatePosition();
			}
		}
	}

	private void Awake()
	{
		photonView = base.gameObject.GetPhotonView();
		if (photonView.isMine || GameState.GameMode != GameMode.Multiplayer)
		{
			carUIControl = UnityEngine.Object.FindObjectOfType<CarUIControl>();
		}
		suspensionController = GetComponent<SuspensionController>();
		propBlock = new MaterialPropertyBlock();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if ((!MeshesMerged && GameState.GameMode == GameMode.Multiplayer && PhotonNetwork.inRoom) || !Washing)
		{
			return;
		}
		Dirtiness = Mathf.Lerp(Dirtiness, 0f, Time.deltaTime * WashingSpeed);
		if (Dirtiness < 0.1f)
		{
			Washing = false;
			Dirtiness = 0f;
			if (MenuManager.Instance.CurrentVehicle != null)
			{
				MenuManager.Instance.DoneWashing();
				MenuManager.Instance.CurrentVehicle.SaveVehicleData();
			}
		}
		UpdateDirtiness();
	}

	public void UpdateDirtiness()
	{
		if (MeshesMerged)
		{
			UpdateMudValuesMerged();
		}
		else
		{
			UpdateMudValuesUnmerged();
		}
	}

	public void UpdateColor(bool Merge)
	{
		if (Application.isPlaying)
		{
			if (Merge)
			{
				MergeBodyParts();
			}
			if (MeshesMerged)
			{
				UpdateBodyColorMerged();
			}
			else
			{
				UpdateBodyColorUnmerged();
			}
			GenerateRimsTexture();
		}
	}

	public void WashVehicle()
	{
		Washing = true;
	}

	public void SetStockModification()
	{
		PartGroup[] array = partGroups;
		foreach (PartGroup group in array)
		{
			InstallBodyPart(group, 0);
		}
		FRimsColor = Color.grey;
		RRimsColor = Color.grey;
		FBeadlocksColor = new Color(0.1f, 0.1f, 0.1f, 1f);
		RBeadlocksColor = new Color(0.1f, 0.1f, 0.1f, 1f);
		AppliedWrapID = 0;
		WrapColor = Color.white;
		WrapCoords = new Vector4(0f, 0f, 1f, 1f);
		WrapLayers = new List<Wrap>();
		IsWrapBaked = false;
		UpdateColor(Merge: false);
		GenerateRimsTexture();
		UpdateEngineModel(EngineType.Stock);
	}

	public void SetRandomModification()
	{
		PartGroup[] array = partGroups;
		foreach (PartGroup partGroup in array)
		{
			InstallBodyPart(partGroup, UnityEngine.Random.Range(0, partGroup.Parts.Length));
		}
		BodyColor = UnityEngine.Random.ColorHSV();
		FRimsColor = UnityEngine.Random.ColorHSV();
		FBeadlocksColor = UnityEngine.Random.ColorHSV();
		RRimsColor = UnityEngine.Random.ColorHSV();
		RBeadlocksColor = UnityEngine.Random.ColorHSV();
		UpdateColor(Merge: false);
	}

	public void AlwaysUseLOD0()
	{
		LODGroup[] componentsInChildren = GetComponentsInChildren<LODGroup>(includeInactive: true);
		foreach (LODGroup lODGroup in componentsInChildren)
		{
			lODGroup.ForceLOD(0);
		}
	}

	public void MergeBodyParts()
	{
		if (!MeshesMerged)
		{
			MeshesMerged = true;
			CombineParts();
			AllRenderers = GetAllRenderers();
			UpdateBodyColorMerged();
			UpdateMudValuesMerged();
		}
	}

	private Renderer[] GetAllRenderers(bool IncludeInactive = false)
	{
		List<Renderer> list = new List<Renderer>();
		List<Shader> list2 = new List<Shader>();
		list2.Add(Shader.Find("Offroad Outlaws/Body"));
		list2.Add(Shader.Find("Offroad Outlaws/Color dirt"));
		list2.Add(Shader.Find("Offroad Outlaws/Diffuse dirt"));
		list2.Add(Shader.Find("Offroad Outlaws/Diffuse dirt with UV0 for dirt"));
		list2.Add(Shader.Find("Offroad Outlaws/Tire"));
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(IncludeInactive);
		foreach (Renderer renderer in componentsInChildren)
		{
			Material[] sharedMaterials = renderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (material != null && list2.Contains(material.shader))
				{
					list.Add(renderer);
					break;
				}
			}
		}
		return list.ToArray();
	}

	private void UpdateBodyColorMerged()
	{
		if (!IsWrapBaked)
		{
			BakeWrap();
		}
		Texture texture = Resources.Load("Wraps/Wrap" + AppliedWrapID) as Texture;
		if (AppliedWrapID == 0)
		{
			texture = null;
		}
		Renderer[] allRenderers = AllRenderers;
		foreach (Renderer renderer in allRenderers)
		{
			renderer.GetPropertyBlock(propBlock);
			propBlock.SetColor("_PaintColor", BodyColor);
			propBlock.SetFloat("_ReflectionStrength", (!GlossyPaint) ? 0.1f : 1.5f);
			propBlock.SetTexture("_BakedWrap", BakedWrap);
			if (texture != null)
			{
				propBlock.SetTexture("_Wrap", texture);
				propBlock.SetColor("_WrapColor", WrapColor);
				propBlock.SetVector("_Wrap_ST", new Vector4(WrapCoords.w, WrapCoords.z, WrapCoords.x, WrapCoords.y));
			}
			renderer.SetPropertyBlock(propBlock);
		}
	}

	public void ChangeCurrentWrap(int ID, Color color, Vector4 coords)
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		Texture texture = Resources.Load("Wraps/Wrap" + ID) as Texture;
		if (ID == 0)
		{
			texture = null;
		}
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.GetPropertyBlock(propBlock);
			propBlock.SetColor("_WrapColor", color);
			if (texture != null)
			{
				propBlock.SetTexture("_Wrap", texture);
			}
			else
			{
				propBlock.SetColor("_WrapColor", Color.clear);
			}
			propBlock.SetVector("_Wrap_ST", new Vector4(coords.w, coords.z, coords.x, coords.y));
			renderer.SetPropertyBlock(propBlock);
		}
		CurrentWrapID = ID;
		CurrentWrapColor = color;
		CurrentWrapCoords = coords;
		AppliedWrapID = ID;
		WrapColor = color;
		WrapCoords = coords;
	}

	public void ClearWraps()
	{
		WrapLayers = new List<Wrap>();
		AppliedWrapID = 0;
		IsWrapBaked = false;
		UpdateBodyColorUnmerged();
	}

	public void BakeWrap()
	{
		RenderTexture renderTexture = new RenderTexture(512, 512, 0);
		BakedWrap = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, mipChain: false);
		Renderer[] allRenderers = GetAllRenderers();
		Material material = new Material(Shader.Find("Offroad Outlaws/Bake"));
		for (int i = 0; i < WrapLayers.Count; i++)
		{
			Texture value = Resources.Load("Wraps/Wrap" + WrapLayers[i].ID) as Texture;
			material.SetTexture("_Wrap" + i, value);
			material.SetColor("_WrapColor" + i, WrapLayers[i].color);
			material.SetTextureOffset("_Wrap" + i, new Vector2(WrapLayers[i].Coords.x, WrapLayers[i].Coords.y));
			material.SetTextureScale("_Wrap" + i, new Vector2(WrapLayers[i].Coords.w, WrapLayers[i].Coords.z));
		}
		if (CurrentWrapID != 0)
		{
			int count = WrapLayers.Count;
			Texture value2 = Resources.Load("Wraps/Wrap" + CurrentWrapID) as Texture;
			material.SetTexture("_Wrap" + count, value2);
			material.SetColor("_WrapColor" + count, CurrentWrapColor);
			material.SetTextureOffset("_Wrap" + count, new Vector2(CurrentWrapCoords.x, CurrentWrapCoords.y));
			material.SetTextureScale("_Wrap" + count, new Vector2(CurrentWrapCoords.w, CurrentWrapCoords.z));
			WrapLayers.Add(new Wrap(CurrentWrapID, CurrentWrapCoords, CurrentWrapColor));
		}
		SessionWrapLayerCount = WrapLayers.Count;
		UnityEngine.Debug.Log("Set session layers to: " + SessionWrapLayerCount);
		if (WrapLayers.Count > WrapLayerCount)
		{
			WrapLayerCount = WrapLayers.Count;
		}
		Graphics.Blit(null, renderTexture, material);
		RenderTexture.active = renderTexture;
		BakedWrap.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
		BakedWrap.Apply();
		RenderTexture.active = null;
		Renderer[] array = allRenderers;
		foreach (Renderer renderer in array)
		{
			renderer.GetPropertyBlock(propBlock);
			propBlock.SetTexture("_BakedWrap", BakedWrap);
			renderer.SetPropertyBlock(propBlock);
		}
		IsWrapBaked = true;
	}

	private void UpdateBodyColorUnmerged()
	{
		Renderer[] allRenderers = GetAllRenderers(IncludeInactive: true);
		Texture texture = Resources.Load("Wraps/Wrap" + AppliedWrapID) as Texture;
		if (AppliedWrapID == 0)
		{
			texture = null;
		}
		if (!IsWrapBaked)
		{
			BakeWrap();
		}
		Renderer[] array = allRenderers;
		foreach (Renderer renderer in array)
		{
			renderer.GetPropertyBlock(propBlock);
			propBlock.SetColor("_PaintColor", BodyColor);
			propBlock.SetFloat("_ReflectionStrength", (!GlossyPaint) ? 0.1f : 1.5f);
			propBlock.SetTexture("_BakedWrap", BakedWrap);
			if (texture != null)
			{
				propBlock.SetTexture("_Wrap", texture);
				propBlock.SetColor("_WrapColor", WrapColor);
				propBlock.SetVector("_Wrap_ST", new Vector4(WrapCoords.w, WrapCoords.z, WrapCoords.x, WrapCoords.y));
			}
			renderer.SetPropertyBlock(propBlock);
		}
	}

	private void UpdateMudValuesUnmerged()
	{
		Renderer[] allRenderers = GetAllRenderers();
		Color dryMudColor = VehicleParts.DryMudColor;
		dryMudColor.a = 1f - Dirtiness;
		Renderer[] array = allRenderers;
		foreach (Renderer renderer in array)
		{
			renderer.GetPropertyBlock(propBlock);
			propBlock.SetColor("_DirtColor", dryMudColor);
			renderer.SetPropertyBlock(propBlock);
		}
	}

	private void UpdateMudValuesMerged()
	{
		Color value = Color.Lerp(VehicleParts.DryMudColor, VehicleParts.WetMudColor, MudWetness);
		value.a = 1f - Dirtiness;
		Renderer[] allRenderers = AllRenderers;
		int num = 0;
		while (true)
		{
			if (num < allRenderers.Length)
			{
				Renderer renderer = allRenderers[num];
				if (renderer == null)
				{
					break;
				}
				renderer.GetPropertyBlock(propBlock);
				propBlock.SetColor("_DirtColor", value);
				renderer.SetPropertyBlock(propBlock);
				num++;
				continue;
			}
			return;
		}
		AllRenderers = GetAllRenderers();
	}

	private BodyPartsData GetBodyPartsData()
	{
		BodyPartsData bodyPartsData = new BodyPartsData();
		bodyPartsData.Dirtiness = Dirtiness;
		bodyPartsData.BodyColor = BodyColor;
		bodyPartsData.FRimsColor = FRimsColor;
		bodyPartsData.FBeadlocksColor = FBeadlocksColor;
		bodyPartsData.RRimsColor = RRimsColor;
		bodyPartsData.RBeadlocksColor = RBeadlocksColor;
		bodyPartsData.Wraps = WrapLayers;
		bodyPartsData.WrapColor = WrapColor;
		bodyPartsData.WrapID = AppliedWrapID;
		bodyPartsData.WrapCoords = WrapCoords;
		bodyPartsData.GlossyPaint = GlossyPaint;
		bodyPartsData.GlossyPaintPurchased = GlossyPaintPurchased;
		PartGroupData[] array = new PartGroupData[partGroups.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = partGroups[i].returnData();
		}
		bodyPartsData.partGroupsData = array;
		return bodyPartsData;
	}

	[ContextMenu("Export data")]
	public string ExportData()
	{
		BodyPartsData bodyPartsData = GetBodyPartsData();
		return XmlSerialization.SerializeData<BodyPartsData>(bodyPartsData);
	}

	public void ImportData(string XmlString)
	{
		BodyPartsData bodyPartsData = new BodyPartsData();
		bodyPartsData = (BodyPartsData)XmlSerialization.DeserializeData<BodyPartsData>(XmlString);
		Dirtiness = bodyPartsData.Dirtiness;
		BodyColor = bodyPartsData.BodyColor;
		FRimsColor = bodyPartsData.FRimsColor;
		FBeadlocksColor = bodyPartsData.FBeadlocksColor;
		RRimsColor = bodyPartsData.RRimsColor;
		RBeadlocksColor = bodyPartsData.RBeadlocksColor;
		WrapLayers = bodyPartsData.Wraps;
		WrapColor = bodyPartsData.WrapColor;
		AppliedWrapID = bodyPartsData.WrapID;
		WrapCoords = bodyPartsData.WrapCoords;
		GlossyPaint = bodyPartsData.GlossyPaint;
		GlossyPaintPurchased = bodyPartsData.GlossyPaintPurchased;
		WrapLayerCount = bodyPartsData.WrapLayerCount;
		if (WrapCoords == Vector4.zero)
		{
			WrapCoords = new Vector4(0f, 0f, 1f, 1f);
		}
		if (AppliedWrapID > 0)
		{
			WrapLayers.Add(new Wrap(AppliedWrapID, WrapCoords, WrapColor));
			AppliedWrapID = 0;
		}
		PartGroup[] array = partGroups;
		foreach (PartGroup partGroup in array)
		{
			PartGroupData[] partGroupsData = bodyPartsData.partGroupsData;
			foreach (PartGroupData partGroupData in partGroupsData)
			{
				if (partGroup.GroupName.Equals(partGroupData.GroupName))
				{
					partGroup.color = partGroupData.color;
					InstallBodyPart(partGroup, partGroupData.InstalledPart);
				}
			}
		}
		if (Winch != null)
		{
			WinchInstalled = Winch.activeSelf;
		}
		if (RepairPack != null && !MeshesMerged)
		{
			RepairPackInstalled = RepairPack.activeSelf;
		}
		if ((GameState.GameMode == GameMode.Multiplayer && photonView.isMine) || GameState.GameMode != GameMode.Multiplayer)
		{
			AlwaysUseLOD0();
		}
		GenerateRimsTexture();
	}

	private void CombineParts()
	{
		if (!Application.isPlaying)
		{
			UnityEngine.Debug.LogError("Combining parts works in play mode only");
			return;
		}
		List<GameObject> list = new List<GameObject>();
		list.Add(BaseMesh);
		PartGroup[] array = partGroups;
		foreach (PartGroup partGroup in array)
		{
			if (partGroup.Parts[partGroup.InstalledPart] != null && partGroup.Parts[partGroup.InstalledPart].activeInHierarchy)
			{
				list.Add(partGroup.Parts[partGroup.InstalledPart]);
			}
		}
		int count = list.Count;
		for (int j = 0; j < count; j++)
		{
			for (int k = 0; k < list[j].transform.childCount; k++)
			{
				if (list[j].transform.GetChild(k).GetComponent<MeshRenderer>() != null && list[j].activeSelf && list[j].transform.GetChild(k).gameObject.activeSelf)
				{
					list.Add(list[j].transform.GetChild(k).gameObject);
					continue;
				}
				list[j].transform.GetChild(k).parent = list[j].transform.parent;
				k--;
			}
		}
		GameObject[] array2 = new GameObject[list.Count];
		array2 = list.ToArray();
		CombineResult combineResult = MeshUtils.Combine(array2);
		ResultMesh = combineResult.GameObject;
		if (combineResult != null)
		{
			MeshCombinerIntegration.RaiseCombined(combineResult.GameObject, combineResult.Mesh);
			combineResult.GameObject.GetComponent<MeshRenderer>().receiveShadows = false;
		}
	}
}
