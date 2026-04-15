using CustomVP;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDataManager : MonoBehaviour
{
	public VehicleType vehicleType;

	public int MoneyPrice;

	public int GoldPrice;

	public float CashPrice;

	public int UpdateID;

	[HideInInspector]
	public int GaragePlaceID;

	public string VehicleID;

	[HideInInspector]
	public bool Bought;

	[HideInInspector]
	public List<string> PurchasedPartsList;

	private CameraController camController;

	private MenuManager menuManager;

	public PunTeams.Team Team = PunTeams.Team.blue;

	[Header("Availability")]
	public Availability vehicleAvailability;

	public DateClass AvailableAfter;

	[Header("Trailer")]
	public bool equipped;

	private TrailerController trailerImOn;

	public float massOnTrailerModifier = 1f;

	private float startMass;

	private ConfigurableJoint joint;

	public bool IsAvailable
	{
		get
		{
			DateTime dateTime = new DateTime(DataStore.GetLong("UpdateOpenedOn" + UpdateID.ToString()));
			StatsData statsData = GameState.LoadStatsData();
			if (statsData.IsMember)
			{
				return true;
			}
			if (vehicleAvailability == Availability.MembersAndEveryoneAfterDate && CurrentDateTime() < dateTime.AddDays(AvailableAfter.Days))
			{
				return false;
			}
			if (vehicleAvailability == Availability.MembersOnly && !statsData.IsMember)
			{
				return false;
			}
			return true;
		}
	}

	public TimeSpan TimeLeft => new DateTime(DataStore.GetLong("UpdateOpenedOn" + UpdateID.ToString())).AddDays(AvailableAfter.Days) - CurrentDateTime();

	[ContextMenu("Turn into dummy car")]
	public void TurnIntoDummyCar()
	{
		UnityEngine.Object.DestroyImmediate(GetComponent<CarController>());
		UnityEngine.Object.DestroyImmediate(GetComponent<CarEffects>());
		UnityEngine.Object.DestroyImmediate(GetComponent<SuspensionController>());
		UnityEngine.Object.DestroyImmediate(GetComponent<PhotonTransformView>());
		UnityEngine.Object.DestroyImmediate(GetComponent<PhotonView>());
		UnityEngine.Object.DestroyImmediate(GetComponent<IKDriverController>());
		UnityEngine.Object.DestroyImmediate(GetComponent<LightsController>());
		UnityEngine.Object.DestroyImmediate(GetComponent<RammingChecker>());
		UnityEngine.Object.DestroyImmediate(GetComponent<EngineController>());
	}

	public void LoadOnTrailer(TrailerController trailer, bool turnToDummy = true)
	{
		AlignOnTrailer(trailer);
		joint = base.gameObject.AddComponent<ConfigurableJoint>();
		joint.connectedBody = trailer.GetComponent<Rigidbody>();
		ConfigurableJoint configurableJoint = joint;
		ConfigurableJointMotion configurableJointMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = configurableJointMotion;
		configurableJoint.xMotion = configurableJointMotion;
		joint.yMotion = ConfigurableJointMotion.Limited;
		if (turnToDummy)
		{
			ConfigurableJoint configurableJoint2 = joint;
			configurableJointMotion = ConfigurableJointMotion.Limited;
			joint.angularZMotion = configurableJointMotion;
			configurableJoint2.angularXMotion = configurableJointMotion;
		}
		else
		{
			ConfigurableJoint configurableJoint3 = joint;
			configurableJointMotion = ConfigurableJointMotion.Locked;
			joint.angularZMotion = configurableJointMotion;
			configurableJoint3.angularXMotion = configurableJointMotion;
		}
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		SoftJointLimit lowAngularXLimit = joint.lowAngularXLimit;
		lowAngularXLimit.limit = -15f;
		joint.lowAngularXLimit = lowAngularXLimit;
		SoftJointLimit highAngularXLimit = joint.highAngularXLimit;
		highAngularXLimit.limit = 15f;
		joint.highAngularXLimit = highAngularXLimit;
		SoftJointLimit angularZLimit = joint.angularZLimit;
		angularZLimit.limit = 25f;
		joint.angularZLimit = angularZLimit;
		SoftJointLimit linearLimit = joint.linearLimit;
		linearLimit.limit = 0.5f;
		joint.linearLimit = linearLimit;
		joint.enableCollision = true;
		if (turnToDummy)
		{
			TurnIntoDummyCar();
			if (startMass == 0f)
			{
				startMass = GetComponent<Rigidbody>().mass;
			}
			GetComponent<Rigidbody>().mass = startMass * massOnTrailerModifier;
		}
		trailerImOn = trailer;
		trailer.VehicleLoadedOnMe(base.gameObject);
	}

	public Vector3 AlignOnTrailer(TrailerController trailer)
	{
		base.transform.rotation = trailer.transform.rotation;
		Vector3 a = Vector3.zero;
		SuspensionController component = GetComponent<SuspensionController>();
		if (component != null)
		{
			GameObject[] getAllWheels = component.GetAllWheels;
			GameObject[] array = getAllWheels;
			foreach (GameObject gameObject in array)
			{
				a += gameObject.transform.position;
			}
			a /= getAllWheels.Length;
		}
		else
		{
			WheelComponent[] componentsInChildren = GetComponentsInChildren<WheelComponent>();
			WheelComponent[] array2 = componentsInChildren;
			foreach (WheelComponent wheelComponent in array2)
			{
				a += wheelComponent.transform.position;
			}
			a /= componentsInChildren.Length;
		}
		Vector3 b = base.transform.position - a;
		base.transform.position = trailer.transform.TransformPoint(trailer.center) + b;
		Utility.AlignHeightOnTrailer(base.transform, trailer);
		return trailer.transform.InverseTransformPoint(base.transform.position);
	}

	public void SaveOnlyGlossinessData()
	{
		VehicleData vehicleData = new VehicleData();
		string @string = DataStore.GetString(VehicleID);
		if (!(@string == string.Empty))
		{
			vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(@string);
			BodyPartsData bodyPartsData = (BodyPartsData)XmlSerialization.DeserializeData<BodyPartsData>(vehicleData.BodyPartsSwitcherXMLData);
			bodyPartsData.GlossyPaint = GetComponent<BodyPartsSwitcher>().GlossyPaint;
			bodyPartsData.GlossyPaintPurchased = GetComponent<BodyPartsSwitcher>().GlossyPaintPurchased;
			string text = vehicleData.BodyPartsSwitcherXMLData = XmlSerialization.SerializeData<BodyPartsData>(bodyPartsData);
			string value = XmlSerialization.SerializeData<VehicleData>(vehicleData);
			DataStore.SetString(VehicleID, value);
		}
	}

	private DateTime CurrentDateTime()
	{
		return DateTime.Now;
	}

	private void Start()
	{
		camController = CameraController.Instance;
		menuManager = MenuManager.Instance;
	}

	public VehicleData GetVehicleData()
	{
		VehicleData vehicleData = new VehicleData();
		if (vehicleType != VehicleType.Trailer)
		{
			vehicleData.SuspensionControllerXMLData = GetComponent<SuspensionController>().ExportData();
			vehicleData.CarControllerXMLData = GetComponent<CarController>().ExportData();
		}
		vehicleData.VehicleName = base.gameObject.name;
		return vehicleData;
	}

	public void SaveDirtinessOnly(int dirtiness)
	{
	}

	[ContextMenu("Save vehicle data")]
	public void SaveVehicleData()
	{
		VehicleData vehicleData = new VehicleData();
		if (vehicleType != VehicleType.Trailer)
		{
			vehicleData.SuspensionControllerXMLData = GetComponent<SuspensionController>().ExportData();
			vehicleData.CarControllerXMLData = GetComponent<CarController>().ExportData();
			vehicleData.BodyPartsSwitcherXMLData = GetComponent<BodyPartsSwitcher>().ExportData();
		}
		vehicleData.PurchasedPartsList = PurchasedPartsList;
		vehicleData.VehicleName = base.gameObject.name;
		vehicleData.equippedTrailer = equipped;
		string value = XmlSerialization.SerializeData<VehicleData>(vehicleData);
		DataStore.SetString(VehicleID, value);
	}

	[ContextMenu("Load vehicle data")]
	public void LoadVehicleData()
	{
		VehicleData vehicleData = new VehicleData();
		string @string = DataStore.GetString(VehicleID);
		if (!(@string == string.Empty))
		{
			vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(@string);
			if (vehicleType != VehicleType.Trailer)
			{
				GetComponent<SuspensionController>().ImportData(vehicleData.SuspensionControllerXMLData);
				GetComponent<BodyPartsSwitcher>().ImportData(vehicleData.BodyPartsSwitcherXMLData);
				GetComponent<CarController>().ImportData(vehicleData.CarControllerXMLData);
			}
			else
			{
				equipped = vehicleData.equippedTrailer;
			}
			PurchasedPartsList = vehicleData.PurchasedPartsList;
		}
	}

	public void LoadVehicleDataFromString(string vehicleDataString)
	{
		VehicleData vehicleData = new VehicleData();
		try
		{
			vehicleData = (VehicleData)XmlSerialization.DeserializeData<VehicleData>(vehicleDataString);
			if (vehicleType != VehicleType.Trailer)
			{
				UnityEngine.Debug.Log("Loading car data from string");
				GetComponent<SuspensionController>().ImportData(vehicleData.SuspensionControllerXMLData);
				GetComponent<BodyPartsSwitcher>().ImportData(vehicleData.BodyPartsSwitcherXMLData);
			}
			else
			{
				equipped = vehicleData.equippedTrailer;
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Could not load vehicle data from string: " + ex.Message);
		}
	}

	private void OnMouseDown()
	{
		if (menuManager != null)
		{
			if (trailerImOn == null)
			{
				menuManager.ChangeCurrentVehicle(this, InstantCameraMove: false);
			}
			else
			{
				menuManager.ChangeCurrentVehicle(trailerImOn.GetComponent<VehicleDataManager>(), InstantCameraMove: false);
			}
		}
	}
}
