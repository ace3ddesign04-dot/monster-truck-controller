using AGS_MonsterTruckControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGS_MTC_SuspensionController : MonoBehaviour
{
	[HideInInspector]
	public AGS_MTC_CarController carController;

	[HideInInspector]
	public bool UpdateSuspensionInUpdate;

	public AGS_MTC_Suspension CurrentFrontSuspension;

	public AGS_MTC_Suspension CurrentRearSuspension;

	public AGS_MTC_WheelsControls FrontWheelsControls;

	public AGS_MTC_WheelsControls RearWheelsControls;

	public GameObject[] SpareWheelHolders;

	public List<AGS_MTC_Suspension> FrontSuspensions;

	public List<AGS_MTC_Suspension> RearSuspensions;

	public int frontSuspension;

	public int rearSuspension;

	public GameObject[] FrontTires;

	public GameObject[] FrontRims;

	public List<Transform> FrontTireSizeBones;

	public GameObject[] RearTires;

	public GameObject[] RearRims;

	public List<Transform> RearTireSizeBones;

	public GameObject[] SpareTires;

	public GameObject[] SpareRims;

	public bool multiplayerTraileredCar;

	public bool multiplayerCar;

	public GameObject[] GetAllWheels
	{
		get
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < FrontRims.Length; i++)
			{
				list.Add(FrontRims[i]);
			}
			for (int j = 0; j < RearRims.Length; j++)
			{
				list.Add(RearRims[j]);
			}
			return list.ToArray();
		}
	}

	private void Update()
	{
		if (UpdateSuspensionInUpdate && carController == null)
		{
			UpdateSuspensions(0f, 0f);
		}
		if (multiplayerTraileredCar)
		{
			UpdateSuspensions(0f, 0f);
		}
	}

	private void Awake()
	{
		carController = GetComponent<AGS_MTC_CarController>();
	}

	public void SetStockWheels()
	{
		FrontWheelsControls.SetStock();
		RearWheelsControls.SetStock();
		LoadWheels();
		DoWheelsSize();
	}

	public void SetRandomWheels()
	{
		FrontWheelsControls.SetRandom(CurrentFrontSuspension);
		RearWheelsControls.SetRandom(CurrentRearSuspension);
		LoadWheels();
		DoWheelsSize();
	}

	public void SetStockSuspensionsValues()
	{
		AGS_MTC_Suspension[] allSuspensions = getAllSuspensions();
		AGS_MTC_Suspension[] array = allSuspensions;
		foreach (AGS_MTC_Suspension suspension in array)
		{
            AGS_MTC_SuspensionValue[] controlValues = suspension.GetControlValues();
			for (int j = 0; j < controlValues.Length; j++)
			{
                AGS_MTC_SuspensionControlLimit limit = AGS_MTC_SuspensionControlLimits.getLimit(suspension.gameObject.name, controlValues[j].ValueName);
				if (limit != null && limit.ModifiableByPlayer)
				{
					controlValues[j].FloatValue = limit.fDef;
					controlValues[j].IntValue = limit.iDef;
				}
			}
			suspension.OnValidate();
		}
		SetStockWheels();
	}

	public void NextFrontSuspension()
	{
		if (frontSuspension <= FrontSuspensions.Count - 2)
		{
			frontSuspension++;
			SetFrontSuspension(frontSuspension);
		}
	}

	public void PrevFrontSuspension()
	{
		if (frontSuspension != 0)
		{
			frontSuspension--;
			SetFrontSuspension(frontSuspension);
		}
	}

	public void NextRearSuspension()
	{
		if (rearSuspension <= RearSuspensions.Count - 2)
		{
			rearSuspension++;
			SetRearSuspension(rearSuspension);
		}
	}

	public void PrevRearSuspension()
	{
		if (rearSuspension != 0)
		{
			rearSuspension--;
			SetRearSuspension(rearSuspension);
		}
	}

	public void UpdateSuspensions(float SteerAngle, float rpm)
	{
		if (CurrentFrontSuspension != null)
		{
			CurrentFrontSuspension.UpdateSuspension(SteerAngle, FrontWheelsControls.WheelsRadius.FloatValue * FrontWheelsControls.DefaultWheelColliderRadius, rpm);
		}
		if (CurrentRearSuspension != null)
		{
			CurrentRearSuspension.UpdateSuspension(SteerAngle, RearWheelsControls.WheelsRadius.FloatValue * RearWheelsControls.DefaultWheelColliderRadius, rpm);
		}
	}

	public void FindSuspensions()
	{
		carController = GetComponent<AGS_MTC_CarController>();
        AGS_MTC_Suspension[] array = new AGS_MTC_Suspension[GetComponentsInChildren<AGS_MTC_Suspension>(includeInactive: true).Length];
		array = GetComponentsInChildren<AGS_MTC_Suspension>(includeInactive: true);
		FrontSuspensions = new List<AGS_MTC_Suspension>();
		RearSuspensions = new List<AGS_MTC_Suspension>();
		AGS_MTC_Suspension[] array2 = array;
		foreach (AGS_MTC_Suspension suspension in array2)
		{
			if (suspension.side == AGS_MTC_Side.Front)
			{
				FrontSuspensions.Add(suspension);
			}
			else
			{
				RearSuspensions.Add(suspension);
			}
		}
		frontSuspension = 0;
		rearSuspension = 0;
		SetFrontSuspension(frontSuspension);
		SetRearSuspension(rearSuspension);
	}

	public void SetRearSuspension(int ID)
	{
		if (RearSuspensions == null || RearSuspensions.Count == 0)
		{
			return;
		}
		for (int i = 0; i < RearSuspensions.Count; i++)
		{
			RearSuspensions[i].gameObject.SetActive(i == ID);
		}
		CurrentRearSuspension = RearSuspensions[ID];
		if (carController != null)
		{
			if (!CurrentRearSuspension.DirtBikeWheels)
			{
				carController.wheels[2].wc = CurrentRearSuspension.wheelColliders[0];
				carController.wheels[3].wc = CurrentRearSuspension.wheelColliders[1];
				SetupCarController(CurrentRearSuspension.wheelColliders.Length + 2);
				if (CurrentRearSuspension.wheelColliders.Length > 2)
				{
					carController.wheels[4].wc = CurrentRearSuspension.wheelColliders[2];
					carController.wheels[5].wc = CurrentRearSuspension.wheelColliders[3];
				}
			}
			else
			{
				carController.wheels[1].wc = CurrentRearSuspension.wheelColliders[0];
			}
		}
		rearSuspension = ID;
		CurrentRearSuspension.OnValidate();
		LoadWheels();
		DoWheelsSize();
	}

	public void SetFrontSuspension(int ID)
	{
		if (FrontSuspensions == null || FrontSuspensions.Count == 0)
		{
			return;
		}
		for (int i = 0; i < FrontSuspensions.Count; i++)
		{
			FrontSuspensions[i].gameObject.SetActive(i == ID);
		}
		CurrentFrontSuspension = FrontSuspensions[ID];
		if (carController != null)
		{
			carController.wheels[0].wc = CurrentFrontSuspension.wheelColliders[0];
			if (CurrentFrontSuspension.wheelColliders.Length > 1)
			{
				carController.wheels[1].wc = CurrentFrontSuspension.wheelColliders[1];
			}
			carController.OnValidate();
		}
		frontSuspension = ID;
		CurrentFrontSuspension.OnValidate();
		LoadWheels();
		DoWheelsSize();
	}

	public void UpdatePrefabs()
	{
        AGS_MTC_Suspension[] allSuspensions = getAllSuspensions();
        AGS_MTC_Suspension[] array = allSuspensions;
		foreach (AGS_MTC_Suspension suspension in array)
		{
			if (Resources.Load("Suspensions/" + suspension.name) != null)
			{
				Vector3 position = suspension.transform.position;
				Quaternion rotation = suspension.transform.rotation;
				Vector3 localScale = suspension.transform.localScale;
				Transform parent = suspension.transform.parent;
				string name = suspension.transform.name;
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Suspensions/" + suspension.name, typeof(GameObject))) as GameObject;
				gameObject.layer = base.gameObject.layer;
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.gameObject.layer = base.gameObject.layer;
				}
				gameObject.transform.parent = parent;
				gameObject.transform.position = position;
				gameObject.transform.rotation = rotation;
				gameObject.transform.localScale = localScale;
				gameObject.name = name;
				gameObject.GetComponent<AGS_MTC_Suspension>().SetControlValues(suspension.GetControlValues());
				suspension.transform.parent = null;
				StartCoroutine(ForcedDestroy(suspension.gameObject));
			}
		}
		FindSuspensions();
		Resources.UnloadUnusedAssets();
	}

	[ContextMenu("Load wheels")]
	public void LoadWheels()
	{
		if (CurrentFrontSuspension != null && (!CurrentFrontSuspension.DontLoadWheels || CurrentFrontSuspension.DirtBikeWheels || CurrentFrontSuspension.ATVWheels))
		{
			if (!FrontWheelsControls.TankTracks)
			{
				LoadFrontWheels();
			}
		}
		if (CurrentRearSuspension != null && (!CurrentRearSuspension.DontLoadWheels || CurrentRearSuspension.DirtBikeWheels || CurrentFrontSuspension.ATVWheels))
		{
            AGS_MTC_SuspensionControlLimit limit = AGS_MTC_SuspensionControlLimits.getLimit(CurrentRearSuspension.gameObject.name, "Rim");
			if (RearWheelsControls.Rim.IntValue > limit.iMax)
			{
				RearWheelsControls.Rim.IntValue = 0;
			}
			if (!RearWheelsControls.TankTracks)
			{
				LoadRearWheels();
			}
		}
		DoWheelsSize();
		if (carController != null)
		{
			carController.FrontInstalledTiresID = FrontWheelsControls.Tire.IntValue;
			carController.RearInstalledTiresID = RearWheelsControls.Tire.IntValue;
		}
	}

	private void LoadFrontWheels()
	{
		if (CurrentFrontSuspension == null || CurrentFrontSuspension.WheelHolders == null)
		{
			return;
		}
		Transform[] wheelHolders = CurrentFrontSuspension.WheelHolders;
		foreach (Transform transform in wheelHolders)
		{
			if (transform != null)
			{
				for (int j = 0; j < transform.childCount; j++)
				{
					UnityEngine.Object.DestroyImmediate(transform.GetChild(j).gameObject);
				}
			}
		}
		if (FrontRims != null && FrontRims.Length > 0)
		{
			for (int k = 0; k < FrontRims.Length; k++)
			{
				UnityEngine.Object.DestroyImmediate(FrontRims[k]);
			}
		}
		FrontRims = new GameObject[CurrentFrontSuspension.WheelHolders.Length];
		string str = "Rim";
		if (CurrentFrontSuspension.DirtBikeWheels)
		{
			str = "BikeRim";
		}
		if (CurrentFrontSuspension.ATVWheels)
		{
			str = "ATVRim";
		}
		for (int l = 0; l < FrontRims.Length; l++)
		{
			FrontRims[l] = (UnityEngine.Object.Instantiate(Resources.Load("Rims/" + str + FrontWheelsControls.Rim.IntValue.ToString(), typeof(GameObject))) as GameObject);
			FrontRims[l].layer = base.gameObject.layer;
			FrontRims[l].transform.parent = CurrentFrontSuspension.WheelHolders[l];
			FrontRims[l].transform.localPosition = Vector3.zero;
			FrontRims[l].transform.localRotation = Quaternion.identity;
			FrontRims[l].transform.localScale = Vector3.one;
		}
		if (FrontTires != null && FrontTires.Length > 0)
		{
			for (int m = 0; m < FrontTires.Length; m++)
			{
				UnityEngine.Object.DestroyImmediate(FrontTires[m]);
			}
		}
		FrontTires = new GameObject[CurrentFrontSuspension.WheelHolders.Length];
		FrontTireSizeBones = new List<Transform>();
		string str2 = "Tire";
		if (CurrentFrontSuspension.DirtBikeWheels)
		{
			str2 = "BikeTire";
		}
		if (CurrentFrontSuspension.ATVWheels)
		{
			str2 = "ATVTire";
		}
		for (int n = 0; n < FrontTires.Length; n++)
		{
			FrontTires[n] = (UnityEngine.Object.Instantiate(Resources.Load("Tires/" + str2 + FrontWheelsControls.Tire.IntValue.ToString(), typeof(GameObject))) as GameObject);
			FrontTires[n].layer = base.gameObject.layer;
			Renderer[] componentsInChildren = FrontTires[n].GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.gameObject.layer = base.gameObject.layer;
			}
			FrontTires[n].transform.parent = CurrentFrontSuspension.WheelHolders[n];
			FrontTires[n].transform.localPosition = Vector3.zero;
			FrontTires[n].transform.localRotation = Quaternion.identity;
			FrontTires[n].transform.localScale = Vector3.one;
			FrontTireSizeBones.Add(FrontTires[n].transform.Find("[BONE]Size"));
		}
	}

	private void LoadRearWheels()
	{
		if (CurrentRearSuspension == null || CurrentRearSuspension.WheelHolders == null)
		{
			return;
		}
		Transform[] wheelHolders = CurrentRearSuspension.WheelHolders;
		foreach (Transform transform in wheelHolders)
		{
			if (transform != null)
			{
				for (int j = 0; j < transform.childCount; j++)
				{
					UnityEngine.Object.DestroyImmediate(transform.GetChild(j).gameObject);
				}
			}
		}
		if (RearRims != null && RearRims.Length > 0)
		{
			for (int k = 0; k < RearRims.Length; k++)
			{
				UnityEngine.Object.DestroyImmediate(RearRims[k]);
			}
		}
		RearRims = new GameObject[CurrentRearSuspension.WheelHolders.Length];
		string str = "Rim";
		if (CurrentRearSuspension.DirtBikeWheels)
		{
			str = "BikeRim";
		}
		if (CurrentRearSuspension.ATVWheels)
		{
			str = "ATVRim";
		}
		for (int l = 0; l < RearRims.Length; l++)
		{
			RearRims[l] = (UnityEngine.Object.Instantiate(Resources.Load("Rims/" + str + RearWheelsControls.Rim.IntValue.ToString(), typeof(GameObject))) as GameObject);
			RearRims[l].layer = base.gameObject.layer;
			RearRims[l].transform.parent = CurrentRearSuspension.WheelHolders[l];
			RearRims[l].transform.localPosition = Vector3.zero;
			RearRims[l].transform.localRotation = Quaternion.identity;
			RearRims[l].transform.localScale = Vector3.one;
		}
		if (RearTires != null && RearTires.Length > 0)
		{
			for (int m = 0; m < RearTires.Length; m++)
			{
				UnityEngine.Object.DestroyImmediate(RearTires[m]);
			}
		}
		RearTires = new GameObject[CurrentRearSuspension.WheelHolders.Length];
		RearTireSizeBones = new List<Transform>();
		string str2 = "Tire";
		if (CurrentRearSuspension.DirtBikeWheels)
		{
			str2 = "BikeTire";
		}
		if (CurrentRearSuspension.ATVWheels)
		{
			str2 = "ATVTire";
		}
		for (int n = 0; n < RearTires.Length; n++)
		{
			RearTires[n] = (UnityEngine.Object.Instantiate(Resources.Load("Tires/" + str2 + RearWheelsControls.Tire.IntValue.ToString(), typeof(GameObject))) as GameObject);
			RearTires[n].layer = base.gameObject.layer;
			Renderer[] componentsInChildren = RearTires[n].GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.gameObject.layer = base.gameObject.layer;
			}
			RearTires[n].transform.parent = CurrentRearSuspension.WheelHolders[n];
			RearTires[n].transform.localPosition = Vector3.zero;
			RearTires[n].transform.localRotation = Quaternion.identity;
			RearTires[n].transform.localScale = Vector3.one;
			RearTireSizeBones.Add(RearTires[n].transform.Find("[BONE]Size"));
		}
	}

	public void DoWheelsSize()
	{
		if (CurrentFrontSuspension != null)
		{
			float floatValue = FrontWheelsControls.WheelsWidth.FloatValue;
			float floatValue2 = FrontWheelsControls.RimSize.FloatValue;
			float floatValue3 = FrontWheelsControls.WheelsRadius.FloatValue;
			if (FrontRims != null && !FrontWheelsControls.TankTracks)
			{
				GameObject[] frontRims = FrontRims;
				foreach (GameObject gameObject in frontRims)
				{
					gameObject.transform.localScale = new Vector3(floatValue, floatValue2 * floatValue3, floatValue2 * floatValue3);
				}
			}
			if (FrontTireSizeBones != null && !FrontWheelsControls.TankTracks)
			{
				foreach (Transform frontTireSizeBone in FrontTireSizeBones)
				{
					frontTireSizeBone.localScale = new Vector3(1f, floatValue2, floatValue2);
				}
			}
			if (FrontTires != null && !FrontWheelsControls.TankTracks)
			{
				GameObject[] frontTires = FrontTires;
				foreach (GameObject gameObject2 in frontTires)
				{
					Transform transform = gameObject2.transform;
					float x = floatValue;
					float y = floatValue3;
					Vector3 vector = base.transform.InverseTransformPoint(gameObject2.transform.position);
					transform.localScale = new Vector3(x, y, (!(vector.x > 0f)) ? floatValue3 : (0f - floatValue3));
				}
			}
			if (carController != null)
			{
                AGS_MTC_WheelComponent[] wheelColliders = CurrentFrontSuspension.wheelColliders;
				foreach (AGS_MTC_WheelComponent wheelComponent in wheelColliders)
				{
					wheelComponent.wheelRadius = FrontWheelsControls.DefaultWheelColliderRadius * floatValue3;
					if (!FrontWheelsControls.TankTracks)
					{
						wheelComponent.wheelRadius = FrontWheelsControls.DefaultWheelColliderRadius * floatValue3;
					}
					else
					{
						wheelComponent.wheelRadius = FrontWheelsControls.DefaultWheelColliderRadius * FrontWheelsControls.TankTracksWheelCollidersRadius;
					}
					wheelComponent.OnValidate();
				}
			}
			if ((FrontRims == null && FrontTires == null) || (FrontRims.Length == 0 && FrontTires.Length == 0))
			{
				if (CurrentFrontSuspension.WheelHolders[0] != null)
				{
					CurrentFrontSuspension.WheelHolders[0].localScale = new Vector3(floatValue, floatValue3, floatValue3);
				}
				if (CurrentFrontSuspension.WheelHolders[1] != null)
				{
					CurrentFrontSuspension.WheelHolders[1].localScale = new Vector3(floatValue, floatValue3, floatValue3);
				}
			}
		}
		if (!(CurrentRearSuspension != null))
		{
			return;
		}
		float floatValue4 = RearWheelsControls.WheelsWidth.FloatValue;
		float floatValue5 = RearWheelsControls.RimSize.FloatValue;
		float floatValue6 = RearWheelsControls.WheelsRadius.FloatValue;
		if (RearRims != null && !RearWheelsControls.TankTracks)
		{
			GameObject[] rearRims = RearRims;
			foreach (GameObject gameObject3 in rearRims)
			{
				gameObject3.transform.localScale = new Vector3(floatValue4, floatValue5 * floatValue6, floatValue5 * floatValue6);
			}
		}
		if (RearTireSizeBones != null && !RearWheelsControls.TankTracks)
		{
			foreach (Transform rearTireSizeBone in RearTireSizeBones)
			{
				rearTireSizeBone.localScale = new Vector3(1f, floatValue5, floatValue5);
			}
		}
		if (RearTires != null && !RearWheelsControls.TankTracks)
		{
			GameObject[] rearTires = RearTires;
			foreach (GameObject gameObject4 in rearTires)
			{
				Transform transform2 = gameObject4.transform;
				float x2 = floatValue4;
				float y2 = floatValue6;
				Vector3 vector2 = base.transform.InverseTransformPoint(gameObject4.transform.position);
				transform2.localScale = new Vector3(x2, y2, (!(vector2.x > 0f)) ? floatValue6 : (0f - floatValue6));
			}
		}
		if (carController != null)
		{
            AGS_MTC_WheelComponent[] wheelColliders2 = CurrentRearSuspension.wheelColliders;
			foreach (AGS_MTC_WheelComponent wheelComponent2 in wheelColliders2)
			{
				if (!RearWheelsControls.TankTracks)
				{
					wheelComponent2.wheelRadius = RearWheelsControls.DefaultWheelColliderRadius * floatValue6;
				}
				else
				{
					wheelComponent2.wheelRadius = RearWheelsControls.DefaultWheelColliderRadius * RearWheelsControls.TankTracksWheelCollidersRadius;
				}
				wheelComponent2.OnValidate();
			}
		}
		if ((RearRims == null && RearTires == null) || (RearRims.Length == 0 && RearTires.Length == 0))
		{
			if (CurrentRearSuspension.WheelHolders[0] != null)
			{
				CurrentRearSuspension.WheelHolders[0].localScale = new Vector3(floatValue4, floatValue6, floatValue6);
			}
			if (CurrentRearSuspension.WheelHolders[1] != null)
			{
				CurrentRearSuspension.WheelHolders[1].localScale = new Vector3(floatValue4, floatValue6, floatValue6);
			}
		}
	}

	private IEnumerator ForcedDestroy(GameObject go)
	{
		yield return new WaitForSeconds(0f);
		UnityEngine.Object.DestroyImmediate(go);
	}

	private void SetupCarController(int WheelsNumber)
	{
		if (WheelsNumber > carController.wheels.Count)
		{
			while (carController.wheels.Count < WheelsNumber)
			{
				carController.wheels.Add(new AGS_MTC_Wheel());
				carController.wheels[carController.wheels.Count - 1].power = carController.RWD;
				carController.wheels[carController.wheels.Count - 1].inverseSteer = true;
				carController.wheels[carController.wheels.Count - 1].handbrake = true;
			}
		}
		if (WheelsNumber < carController.wheels.Count)
		{
			while (carController.wheels.Count > WheelsNumber)
			{
				carController.wheels.RemoveAt(carController.wheels.Count - 1);
			}
		}
	}

	private AGS_MTC_Suspension[] getAllSuspensions()
	{
		List<AGS_MTC_Suspension> list = new List<AGS_MTC_Suspension>();
		for (int i = 0; i < FrontSuspensions.Count; i++)
		{
			list.Add(FrontSuspensions[i]);
		}
		for (int j = 0; j < RearSuspensions.Count; j++)
		{
			list.Add(RearSuspensions[j]);
		}
		return list.ToArray();
	}

	private AGS_MTC_SuspensionControllerData getSuspensionControllerData()
	{
        AGS_MTC_SuspensionControllerData suspensionControllerData = new AGS_MTC_SuspensionControllerData();
        AGS_MTC_Suspension[] allSuspensions = getAllSuspensions();
		suspensionControllerData.AllSuspensionsDatas = new AGS_MTC_SuspensionData[allSuspensions.Length];
		for (int i = 0; i < allSuspensions.Length; i++)
		{
			suspensionControllerData.AllSuspensionsDatas[i] = new AGS_MTC_SuspensionData();
			suspensionControllerData.AllSuspensionsDatas[i].AllValues = allSuspensions[i].GetControlValues();
			suspensionControllerData.AllSuspensionsDatas[i].UpgradeStage = allSuspensions[i].UpgradeStage;
			suspensionControllerData.AllSuspensionsDatas[i].SuspensionName = allSuspensions[i].SuspensionName;
		}
		suspensionControllerData.SelectedFrontSuspension = frontSuspension;
		suspensionControllerData.SelectedRearSuspension = rearSuspension;
		suspensionControllerData.FrontWheelsControls = FrontWheelsControls;
		suspensionControllerData.RearWheelsControls = RearWheelsControls;
		return suspensionControllerData;
	}

	private AGS_MTC_SuspensionValue[] ClampSuspensionValues(AGS_MTC_SuspensionValue[] input, string suspensionName)
	{
		foreach (AGS_MTC_SuspensionValue suspensionValue in input)
		{
            AGS_MTC_SuspensionControlLimit limit = AGS_MTC_SuspensionControlLimits.getLimit(suspensionName, suspensionValue.ValueName);
			if (limit != null && suspensionValue.valueType == AGS_MTC_ValueType.Float)
			{
				suspensionValue.FloatValue = Mathf.Clamp(suspensionValue.FloatValue, limit.fMin, limit.fMax);
			}
		}
		return input;
	}

	private AGS_MTC_WheelsControls ClampWheelSizes(AGS_MTC_WheelsControls wheelControls, string suspensionName)
	{
		AGS_MTC_SuspensionControlLimit limit = AGS_MTC_SuspensionControlLimits.getLimit(suspensionName, "Wheels radius");
		if (limit != null)
		{
			wheelControls.WheelsRadius.FloatValue = Mathf.Clamp(wheelControls.WheelsRadius.FloatValue, limit.fMin, limit.fMax);
		}
		AGS_MTC_SuspensionControlLimit limit2 = AGS_MTC_SuspensionControlLimits.getLimit(suspensionName, "Wheels width");
		if (limit2 != null)
		{
			wheelControls.WheelsWidth.FloatValue = Mathf.Clamp(wheelControls.WheelsWidth.FloatValue, limit2.fMin, limit2.fMax);
		}
		AGS_MTC_SuspensionControlLimit limit3 = AGS_MTC_SuspensionControlLimits.getLimit(suspensionName, "Rim size");
		if (limit3 != null)
		{
			wheelControls.RimSize.FloatValue = Mathf.Clamp(wheelControls.RimSize.FloatValue, limit3.fMin, limit3.fMax);
		}
		return wheelControls;
	}

	[ContextMenu("Set crazy wheels")]
	private void asd()
	{
		FrontWheelsControls.WheelsRadius.FloatValue = 2.5f;
		DoWheelsSize();
	}
}
