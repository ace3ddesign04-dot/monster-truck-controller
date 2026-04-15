using CustomVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynoRoomController : MonoBehaviour
{
	public static DynoRoomController Instance;

	public WMG_Axis_Graph graph;

	public WMG_Series Line0;

	public WMG_Series Line1;

	public Transform CarPos;

	public LineRenderer[] RStraps;

	public LineRenderer[] FStraps;

	public Transform FrontDynoStand;

	public Transform RearDynoStand;

	public Transform[] Rollers;

	public AnimationCurve GasTruckCurve;

	public AnimationCurve DieselTruckCurve;

	public AnimationCurve AtvCurve;

	public AnimationCurve BikeCurve;

	public AnimationCurve CrawlersCurve;

	public AnimationCurve UtvCurve;

	public float TorqueCurveMultiplier = 0.5f;

	public float TrucksHPMultiplier = 2f;

	public float AtvsHPMultiplier = 2f;

	public float UtvsHPMultiplier = 2f;

	public float BikesHPMultiplier = 2f;

	public float CrawlersHPMultiplier = 2f;

	private float DynoRatio;

	private CarController car;

	private Rigidbody carRigidbody;

	private EngineController engineController;

	private Coroutine dynoRoutine;

	private List<Vector2> HPCurve;

	private List<Vector2> TQCurve;

	public DynoRoomController()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	public void InitializeDyno(GameObject Vehicle)
	{
		car = Vehicle.GetComponent<CarController>();
		carRigidbody = Vehicle.GetComponent<Rigidbody>();
		engineController = Vehicle.GetComponent<EngineController>();
		engineController.enabled = true;
		car.transform.position = CarPos.position;
		car.transform.rotation = CarPos.rotation;
		Vector3 position = FrontDynoStand.position;
		Vector3 position2 = car.wheels[0].wc.transform.position;
		position.z = position2.z;
		FrontDynoStand.position = position;
		Vector3 position3 = RearDynoStand.position;
		Vector3 position4 = car.wheels[car.wheels.Count - 1].wc.transform.position;
		position3.z = position4.z;
		RearDynoStand.position = position3;
		car.enabled = true;
		car.FWD = (car.RWD = true);
		car.PreventFromSideSliding = false;
		car.OnValidate();
	}

	private void BuildCurve(float PeakHP)
	{
		VehicleDataManager component = car.GetComponent<VehicleDataManager>();
		AnimationCurve animationCurve = null;
		float num = 0f;
		switch (component.vehicleType)
		{
		case VehicleType.ATV:
			animationCurve = AtvCurve;
			num = AtvsHPMultiplier;
			break;
		case VehicleType.Bike:
			animationCurve = BikeCurve;
			num = BikesHPMultiplier;
			break;
		case VehicleType.Crawler:
			animationCurve = CrawlersCurve;
			num = CrawlersHPMultiplier;
			break;
		case VehicleType.SideBySide:
			animationCurve = UtvCurve;
			num = UtvsHPMultiplier;
			break;
		case VehicleType.Truck:
			num = TrucksHPMultiplier;
			animationCurve = ((!engineController.Diesel) ? GasTruckCurve : DieselTruckCurve);
			break;
		}
		HPCurve = new List<Vector2>();
		TQCurve = new List<Vector2>();
		PeakHP *= num;
		float axisMaxValue = PeakHP * 1.1f;
		float num2 = 0f;
		for (int i = 0; i < animationCurve.keys.Length; i++)
		{
			if (animationCurve.keys[i].value > num2)
			{
				num2 = animationCurve.keys[i].value;
			}
		}
		float num3 = PeakHP / num2;
		for (int j = 0; j < animationCurve.keys.Length; j++)
		{
			float time = animationCurve.keys[j].time;
			float num4 = animationCurve.keys[j].value * num3;
			float y = 5252f * num4 / time * TorqueCurveMultiplier;
			HPCurve.Add(new Vector2(time, num4));
			TQCurve.Add(new Vector2(time, y));
		}
		float num5 = 0f;
		foreach (Vector2 item in TQCurve)
		{
			Vector2 current = item;
			if (current.y > num5)
			{
				num5 = current.y;
			}
		}
		float num6 = 0f;
		foreach (Vector2 item2 in TQCurve)
		{
			Vector2 current2 = item2;
			num6 += current2.y;
		}
		num6 /= (float)TQCurve.Count;
		float num7 = 0f;
		foreach (Vector2 item3 in HPCurve)
		{
			Vector2 current3 = item3;
			num7 += current3.y;
		}
		num7 /= (float)HPCurve.Count;
		DynoFinished(PeakHP, num7, num5, num6);
		graph.Start();
		Line0.pointValues = new WMG_List<Vector2>();
		Line1.pointValues = new WMG_List<Vector2>();
		graph.yAxis.AxisMaxValue = axisMaxValue;
		foreach (Vector2 item4 in HPCurve)
		{
			Line0.pointValues.Add(item4);
		}
		foreach (Vector2 item5 in TQCurve)
		{
			Line1.pointValues.Add(item5);
		}
	}

	private void DynoFinished(float maxHP, float avgHP, float maxTQ, float avgTQ)
	{
		MenuManager.Instance.DynoFinished(maxHP, avgHP, maxTQ, avgTQ);
	}

	private void Update()
	{
		if (car == null)
		{
			return;
		}
		foreach (_Wheel wheel in car.wheels)
		{
			wheel.wc.wheelCollider.FakeRPM = 15f * (DynoRatio + 0.3f);
		}
		if (car.wheels[0].wc.IsGrounded)
		{
			carRigidbody.AddRelativeTorque(-Vector3.right * 5000f * DynoRatio);
		}
		engineController.FakeRPMTarget = Mathf.Lerp(800f, 6000f, DynoRatio);
		Transform[] rollers = Rollers;
		foreach (Transform transform in rollers)
		{
			transform.Rotate(0f, 0f, 20f * (DynoRatio + 0.3f));
		}
		Vector3 position = Vector3.zero;
		Vector3 position2 = Vector3.zero;
		if (car.GetComponent<BodyPartsSwitcher>().RearWinchPoint != null)
		{
			position = car.GetComponent<BodyPartsSwitcher>().RearWinchPoint.position;
		}
		else
		{
			if (car.wheels.Count == 4)
			{
				position = (car.wheels[2].wc.transform.position + car.wheels[3].wc.transform.position) / 2f;
			}
			if (car.wheels.Count == 2)
			{
				position = car.wheels[1].wc.transform.position;
			}
		}
		if (car.GetComponent<BodyPartsSwitcher>().FrontWinchPoint != null)
		{
			position2 = car.GetComponent<BodyPartsSwitcher>().FrontWinchPoint.position;
		}
		else
		{
			if (car.wheels.Count == 4)
			{
				position2 = (car.wheels[0].wc.transform.position + car.wheels[1].wc.transform.position) / 2f;
			}
			if (car.wheels.Count == 2)
			{
				position2 = car.wheels[0].wc.transform.position;
			}
		}
		LineRenderer[] rStraps = RStraps;
		foreach (LineRenderer lineRenderer in rStraps)
		{
			lineRenderer.SetPosition(1, position);
		}
		LineRenderer[] fStraps = FStraps;
		foreach (LineRenderer lineRenderer2 in fStraps)
		{
			lineRenderer2.SetPosition(1, position2);
		}
	}

	[ContextMenu("Start")]
	public void StartDyno()
	{
		if (dynoRoutine != null)
		{
			StopCoroutine(dynoRoutine);
		}
		dynoRoutine = StartCoroutine(DoDyno());
	}

	private IEnumerator DoDyno()
	{
		for (float f2 = 0f; f2 <= 1f; f2 += 0.01f)
		{
			DynoRatio = f2;
			yield return null;
		}
		DynoRatio = 1f;
		yield return new WaitForSeconds(3f);
		float maxHP = car.GetMaxTorque();
		BuildCurve(maxHP);
		for (float f = 1f; f >= 0f; f -= 0.005f)
		{
			DynoRatio = f;
			yield return null;
		}
		DynoRatio = 0f;
		dynoRoutine = null;
	}
}
